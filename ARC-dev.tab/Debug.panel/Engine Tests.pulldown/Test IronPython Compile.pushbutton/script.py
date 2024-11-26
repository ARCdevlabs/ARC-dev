import sys
import os.path as op
import clr

from pyrevit import USER_SYS_TEMP
from pyrevit import script
from pyrevit.framework import IO

# compile
try:
    source = script.get_bundle_file('ReadPassCode241125.py')
    dest = op.join(USER_SYS_TEMP, 'ReadPassCode241125.dll')
    print dest
    clr.CompileModules(dest, source)
except IO.IOException as ioerr:
    print('DLL file already exists...')
except Exception as cerr:
    print('Compilation failed: {}'.format(cerr))

# import test
sys.path.append(USER_SYS_TEMP)
clr.AddReferenceToFileAndPath(dest)

# import ipycompiletest

# ipycompiletest.compile_test('Compiled function works.')

# ipycompiletest.CompiledType('Compiled type works.')

uidoc = __revit__.ActiveUIDocument
doc = uidoc.Document
import ReadPassCode241125
pass_code = ReadPassCode241125
print pass_code.read_pass_code()