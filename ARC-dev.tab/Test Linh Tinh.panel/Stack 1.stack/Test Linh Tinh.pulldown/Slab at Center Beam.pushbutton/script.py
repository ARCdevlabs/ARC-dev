# -*- coding: utf-8 -*-
import Autodesk
from Autodesk.Revit.DB import *
import Autodesk.Revit.DB as DB
from System.Collections.Generic import *
from pyrevit import revit
# -*- coding: utf-8 -*-
import sys
import os
# Tìm đường dẫn thư mục pyRevit Extensions
pyrevit_extensions_path = os.path.join(os.getenv('APPDATA'), 'pyRevit', 'Extensions')
# hoặc sử dụng PROGRAMDATA nếu thư viện được cài đặt ở đó
# pyrevit_extensions_path = os.path.join(os.getenv('PROGRAMDATA'), 'pyRevit', 'Extensions')
# Đường dẫn thư viện nances
nances_lib_path = os.path.join(pyrevit_extensions_path, 'ARC extension.extension', 'lib')
# Thêm đường dẫn vào sys.path nếu chưa có
if nances_lib_path not in sys.path:
    sys.path.append(nances_lib_path)
    import nances as module



RESOLUTION_TYPES = [DB.FailureResolutionType.MoveElements,
                    DB.FailureResolutionType.CreateElements,
                    DB.FailureResolutionType.DetachElements,
                    DB.FailureResolutionType.FixElements,
                    DB.FailureResolutionType.UnlockConstraints,
                    DB.FailureResolutionType.SkipElements,
                    DB.FailureResolutionType.DeleteElements,
                    DB.FailureResolutionType.QuitEditMode,
                    DB.FailureResolutionType.SetValue,
                    DB.FailureResolutionType.SaveDocument]


# see FailureProcessingResult docs
# http://www.revitapidocs.com/2018.1/f147e6e6-4b2e-d61c-df9b-8b8e5ebe3fcb.htm
# explains usage of FailureProcessingResult options
class FailureSwallower(DB.IFailuresPreprocessor):
    def __init__(self, log_errors=True):
        self._logerror = log_errors
        self._failures_swallowed = []

    def _set_and_resolve(self, failuresAccessor, failure, res_type):
        failure_id = failure.GetFailureDefinitionId()
        failure_guid = getattr(failure_id, 'Guid', '')
        # mark as swallowed
        try:
            failure.SetCurrentResolutionType(res_type)
            failuresAccessor.ResolveFailure(failure)
            
            self._failures_swallowed.append(failure_id)
        except:
            pass

    def get_swallowed_failures(self):
        failures = set()
        failure_reg = HOST_APP.app.GetFailureDefinitionRegistry()
        if failure_reg:
            for failure_id in self._failures_swallowed:
                failure_obj = failure_reg.FindFailureDefinition(failure_id)
                if failure_obj:
                    failures.add(failure_obj)

        return failures

    def reset(self):
        """Reset swallowed errors"""
        self._failures_swallowed = []

    def preprocess_failures(self, failure_accessor):
        """Pythonic wrapper for `PreprocessFailures` interface method"""
        return self.PreprocessFailures(failure_accessor)

    def PreprocessFailures(self, failuresAccessor):
        """Required IFailuresPreprocessor interface method"""
        severity = failuresAccessor.GetSeverity()


        if severity == coreutils.get_enum_none(DB.FailureSeverity):

            return DB.FailureProcessingResult.Continue

        # log the failure messages
        failures = failuresAccessor.GetFailureMessages()


        # go through failures and attempt resolution
        action_taken = False
        for failure in failures:

            failure_id = failure.GetFailureDefinitionId()
            failure_guid = getattr(failure_id, 'Guid', '')
            failure_severity = failure.GetSeverity()
            failure_desc = failure.GetDescriptionText()
            failure_has_res = failure.HasResolutions()


            # if it's a warning and does not have any resolution
            # delete it! it might have a popup window
            if not failure_has_res \
                    and failure_severity == DB.FailureSeverity.Warning:
                failuresAccessor.DeleteWarning(failure)
                continue

            # find failure definition id
            # at this point the failure_has_res is True
            failure_def_accessor = get_failure_by_id(failure_id)
            default_res = failure_def_accessor.GetDefaultResolutionType()

            # iterate through resolution options, pick one and resolve
            for res_type in RESOLUTION_TYPES:
                if default_res == res_type:

                    self._set_and_resolve(failuresAccessor, failure, res_type)
                    action_taken = True
                    break
                elif failure.HasResolutionOfType(res_type):

                    self._set_and_resolve(failuresAccessor, failure, res_type)
                    # marked as action taken
                    action_taken = True
                    break

        # report back
        if action_taken:
            return DB.FailureProcessingResult.ProceedWithCommit
        else:
            return DB.FailureProcessingResult.Continue

def get_failure_by_guid(failure_guid):
    fdr = HOST_APP.app.GetFailureDefinitionRegistry()
    fgid = framework.Guid(failure_guid)
    fid = DB.FailureDefinitionId(fgid)
    return fdr.FindFailureDefinition(fid)


def get_failure_by_id(failure_id):
    fdr = HOST_APP.app.GetFailureDefinitionRegistry()
    return fdr.FindFailureDefinition(failure_id)



DEFAULT_TRANSACTION_NAME = 'ARC Transaction'
class Transaction():
    def __init__(self, name=None,
                 doc=None,
                 clear_after_rollback=False,
                 show_error_dialog=False,
                 swallow_errors=False,
                 nested=False):
        doc = doc
        # create nested transaction if one is already open
        if doc.IsModifiable or nested:
            self._rvtxn = \
                DB.SubTransaction(doc)
        else:
            self._rvtxn = \
                DB.Transaction(doc, name if name else DEFAULT_TRANSACTION_NAME)
            self._fhndlr_ops = self._rvtxn.GetFailureHandlingOptions()
            self._fhndlr_ops = \
                self._fhndlr_ops.SetClearAfterRollback(clear_after_rollback)
            self._fhndlr_ops = \
                self._fhndlr_ops.SetForcedModalHandling(show_error_dialog)
            if swallow_errors:
                self._fhndlr_ops = \
                    self._fhndlr_ops.SetFailuresPreprocessor(
                        failure.FailureSwallower()
                        )
            self._rvtxn.SetFailureHandlingOptions(self._fhndlr_ops)

    def __enter__(self):
        self._rvtxn.Start()
        return self

    def __exit__(self, exception, exception_value, traceback):
        if exception:
            self._rvtxn.RollBack()

        else:
            try:
                self._rvtxn.Commit()
            except Exception as errmsg:
                self._rvtxn.RollBack()

    @property
    def name(self):
        if hasattr(self._rvtxn, 'GetName'):
            return self._rvtxn.GetName()

    @name.setter
    def name(self, new_name):
        if hasattr(self._rvtxn, 'SetName'):
            return self._rvtxn.SetName(new_name)

    @property
    def status(self):
        return self._rvtxn.GetStatus()

    def has_started(self):
        return self._rvtxn.HasStarted()

    def has_ended(self):
        return self._rvtxn.HasEnded()












if module.AutodeskData():
    uidoc = __revit__.ActiveUIDocument
    doc = uidoc.Document
    active_view = module.Active_view(doc)
    view_direction = active_view.ViewDirection
    if view_direction.Z == 1 and str(active_view.ViewType) != "ThreeD":

        beams = module.get_elements(uidoc,doc, 'Select Beams', noti = False)
        try:
            beam = beams[0]
            # beam_level = module.get_parameter_value_by_name(beam, "Reference Level", is_UTF8 = False)
            beam_level = module.get_builtin_parameter_by_name(beam, DB.BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM).AsElementId()


            def create_wall(doc, curve, level):
                wall = Wall.Create(doc, curve, level.Id, False)                         
                return wall  
            list_type_walls = module.all_type_of_class_and_OST(doc, WallType, BuiltInCategory.OST_Walls)
            for wall in list_type_walls:
                type_wall = wall

            trans_group = TransactionGroup(doc, "Create slab")
            trans_group.Start()

            with revit.Transaction("Tạo room separator", swallow_errors=False):
                levels = module.get_all_elements_of_OST(doc, BuiltInCategory.OST_Levels)
                for level in levels:
                    # level_name = module.get_parameter_value_by_name(level, "Name", is_UTF8 = True)
                    # level_name = module.get_builtin_parameter_by_name(level.Id, DB.BuiltInParameter.DATUM_TEXT).AsString()
                    if level.Id == beam_level:
                        room_level = level
                        break
                level_plane = room_level.GetPlaneReference()
                sketch_plane = SketchPlane.Create(doc, level_plane)
                curve_array = Autodesk.Revit.DB.CurveArray()
                list_wall = []
                for i in beams:
                    Cur = i.Location.Curve
                    Startpoint = Cur.GetEndPoint(0)
                    Endpoint = Cur.GetEndPoint(1)
                    Midpoint0 = Cur.Evaluate(0.7, True)
                    PlaMidpoint0 = XYZ(Midpoint0.X, Midpoint0.Y, 0)
                    Zpoint = Autodesk.Revit.DB.XYZ(Midpoint0.X, Midpoint0.Y, Midpoint0.Z + 10)
                    Zaxis = Autodesk.Revit.DB.Line.CreateBound(Midpoint0, Zpoint)
                    LCenter = Autodesk.Revit.DB.Line.CreateBound(XYZ(Startpoint.X,Startpoint.Y,Startpoint.Z),XYZ(Endpoint.X,Endpoint.Y,Startpoint.Z))
                    curve_array.Append(LCenter)
                    # wall = create_wall(doc, LCenter, level)
                    # list_wall.append(wall)
                room_separation = doc.Create.NewRoomBoundaryLines(sketch_plane, curve_array, active_view)

                # options = t1.GetFailureHandlingOptions()
                # options.SetClearAfterRollback(True)
                # options.SetForcedModalHandling(False)
                # t1.SetFailureHandlingOptions(options)



            limitoffset = 8
            beam_phase = module.get_parameter_value_by_name(beam, "Phase Created", is_UTF8 = False)
            phases = module.get_all_elements_of_OST(doc, BuiltInCategory.OST_Phases)
            for phase in phases:
                phase_name = module.get_parameter_value_by_name(phase, "Name", is_UTF8 = False)
                if phase_name == beam_phase:
                    room_phase = phase
                    break
            rooms = []
            list_rooms = []

            with revit.Transaction("Đặt room hàng loạt", swallow_errors=True):
                planTopology = doc.get_PlanTopology(room_level)
                for i,plancircuit in enumerate(planTopology.Circuits):
                    if plancircuit.IsRoomLocated == True:
                        continue
                    room = doc.Create.NewRoom(room_phase)
                    room.Name = "Room for create slab" + str(i)
                    room.Number = str(i) + "." + str(i)
                    room.LimitOffset = limitoffset
                    try:
                        new_room = doc.Create.NewRoom(room,plancircuit)
                    except:
                        continue
                    rooms.append(new_room)
                list_rooms.append(rooms)
                list_rooms_flat = module.flatten_list(list_rooms)

            def create_slabs(rooms, floor_type, offset, level_Id):
                slabs = []
                for room in rooms:
                    # IGNORE NON-BOUNDING ROOMS
                    if not room.get_Parameter(BuiltInParameter.ROOM_AREA).AsDouble():
                        return None

                    # ROOM BOUNDARIES -> List[CurveLoop]()
                    room_boundaries = room.GetBoundarySegments(SpatialElementBoundaryOptions())
                    curveLoopList   = List[CurveLoop]()

                    for roomBoundary in room_boundaries:
                        room_curve_loop = CurveLoop()
                        for boundarySegment in roomBoundary:
                            curve = boundarySegment.GetCurve()
                            room_curve_loop.Append(curve)
                        curveLoopList.Add(room_curve_loop)

                    if curveLoopList:
                        slab = Autodesk.Revit.DB.Floor.Create(doc, curveLoopList, floor_type, level_Id)
                        slabs.append(slab)
                        # SET OFFSET
                        param = slab.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM)
                        param.Set(offset)
                        # structure = slab.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL).AsInteger()
                        structure = module.get_builtin_parameter_by_name(slab, DB.BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL)
                        structure.Set(1)
                return slabs


            # t3 = Transaction (doc, "Create slab (done)")
            # t3.Start()
            list_type_floors = module.all_type_of_class_and_OST(doc, FloorType, BuiltInCategory.OST_Floors)
            for floor in list_type_floors:
                type_floor = floor
            type_floor_id = type_floor.Id
            height_offset = 0
            level_id = room_level.Id
            with revit.Transaction("Tạo sàn và xóa room separator và xóa room", swallow_errors=True):
                try:
                    list_new_slabs = create_slabs (list_rooms_flat, type_floor_id, height_offset, level_id)
                    for delete_room_line in room_separation:
                        Autodesk.Revit.DB.Document.Delete(doc,delete_room_line.Id)
                    # for delete_wall in list_wall:
                    #     Autodesk.Revit.DB.Document.Delete(doc,delete_wall.Id)
                    for delete_room in list_rooms_flat:
                        Autodesk.Revit.DB.Document.Delete(doc,delete_room.Id)
                except:
                    pass
            trans_group.Assimilate()
        except:
            module.message_box("Please Select Only Beam")
            pass
    else:
        module.message_box("Please use the tool in plan view.") 