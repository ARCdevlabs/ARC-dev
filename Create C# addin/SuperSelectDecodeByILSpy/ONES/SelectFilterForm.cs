using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Microsoft.Office.Interop.Excel;
using ONES.Properties;

namespace ONES;

public class SelectFilterForm : System.Windows.Forms.Form
{
	private UIDocument uidoc;

	private string textCount;

	private Document doc;

	private Autodesk.Revit.DB.View view;

	private bool formLoad = false;

	private string searchContain = "Contains";

	private string searchEqual = "Equals";

	private string searchStart = "Starts with";

	private string searchEnd = "Ends with";

	private int excelRow = 1;

	private bool advanced = false;

	private IContainer components = null;

	private TreeView treeViewElements;

	private Label labelCount;

	private GroupBox groupBoxSelection;

	private RadioButton rBSelAll;

	private RadioButton rBSelCurrent;

	private RadioButton rBSelView;

	private Button buttonSelect;

	private Button buttonCheck;

	private Button buttonUncheck;

	private Button buttonExpand;

	private Button buttonCollapse;

	private GroupBox groupBoxCondition;

	private CheckBox checkBoxExWorkset;

	private CheckBox checkBoxExFamily;

	private CheckBox checkBoxExType;

	private CheckBox checkBoxExCategory;

	private Button buttonDeselect;

	private CheckBox checkBoxExJoined;

	private CheckBox checkBoxHost;

	private CheckBox checkBoxExNestOfSel;

	private CheckBox checkBoxNested;

	private CheckBox checkBoxExHostOfSel;

	private GroupBox groupBoxExtended;

	private Button buttonExtend;

	private GroupBox groupBoxSelMethod;

	private RadioButton rBNewSel;

	private RadioButton rBExtendSel;

	private GroupBox groupBoxSelectionRange;

	private RadioButton rBAllProjectE;

	private RadioButton rBCurrentViewE;

	private GroupBox groupBoxListingOption;

	private RadioButton rBListType;

	private RadioButton rBListCreated;

	private RadioButton rBListWorkset;

	private RadioButton rBListLevel;

	private RadioButton rBListChanged;

	private RadioButton rBListOwner;

	private GroupBox groupBoxSearch;

	private System.Windows.Forms.ComboBox comboBoxSearch;

	private Button buttonReset;

	private Button buttonSearch;

	private System.Windows.Forms.TextBox textBoxSearch;

	private RadioButton rBSearchFamily;

	private RadioButton rBSearchType;

	private Button buttonExcel;

	private Button buttonClose;

	private RadioButton rBListPhaseCreated;

	private RadioButton rBListPhaseDemolished;

	private GroupBox groupBoxFiltering;

	private Button buttonRefresh;

	private CheckBox checkBoxWarnings;

	private CheckBox checkBoxInplace;

	private Button buttonAdvanced;

	private RadioButton rBListMaterial;

	private CheckBox checkBoxExNested;

	private CheckBox checkBoxExHost;

	private CheckBox checkBoxInstance;

	private Button buttonRemove;

	public List<ElementId> checkedIds { get; set; }

	public SelectFilterForm(UIDocument _uidoc)
	{
		InitializeComponent();
		uidoc = _uidoc;
		doc = uidoc.Document;
		view = uidoc.ActiveView;
		textCount = "Total Selection:";
		if (doc.Application.Language.ToString() == "Japanese")
		{
			JapaneseUI();
		}
		labelCount.Text = textCount;
		rBSelView.Checked = true;
		rBCurrentViewE.Checked = true;
		rBNewSel.Checked = true;
		rBListType.Checked = true;
		rBSearchType.Checked = true;
		base.AcceptButton = buttonSelect;
		base.CancelButton = buttonClose;
		base.MaximizeBox = false;
		buttonAdvanced.Visible = false;
		buttonRemove.Visible = false;
	}

	private void SelectFilterForm_Load(object sender, EventArgs e)
	{
		RefreshTreeView(GetElementsForTreeView());
		formLoad = true;
		comboBoxSearch.Items.Add(searchContain);
		comboBoxSearch.Items.Add(searchEqual);
		comboBoxSearch.Items.Add(searchStart);
		comboBoxSearch.Items.Add(searchEnd);
		comboBoxSearch.SelectedIndex = 0;
		Selection selection = uidoc.Selection;
		ICollection<ElementId> elementIds = selection.GetElementIds();
		if (elementIds.Count == 0)
		{
			groupBoxExtended.Enabled = false;
		}
		ToolTips();
	}

	private void JapaneseUI()
	{
		textCount = "合計選択項目：";
		buttonSelect.Text = "選択";
		buttonDeselect.Text = "選択解除";
		buttonClose.Text = "閉じる";
		buttonRefresh.Text = "更新";
		groupBoxSelection.Text = "選択オプション";
		rBSelCurrent.Text = "現在の選択";
		rBSelView.Text = "ビュー内の表示";
		rBSelAll.Text = "プロジェクト全体";
		groupBoxListingOption.Text = "グループ化オプション";
		rBListType.Text = "タイプ";
		rBListWorkset.Text = "ワークセット";
		rBListLevel.Text = "レベル";
		rBListCreated.Text = "作成者ごと";
		rBListOwner.Text = "所有者";
		rBListChanged.Text = "変更者ごと";
		rBListPhaseCreated.Text = "構築フェーズ";
		rBListPhaseDemolished.Text = "解体フェーズ";
		rBListMaterial.Text = "マテリアル";
		checkBoxInstance.Text = "インスタンスの表示";
		groupBoxFiltering.Text = "フィルタオプション";
		checkBoxInplace.Text = "インプレースのみ";
		checkBoxWarnings.Text = "警告がある要素";
		checkBoxNested.Text = "ネストファミリ";
		checkBoxHost.Text = "ホストファミリ";
		groupBoxSearch.Text = "検索オプション";
		rBSearchType.Text = "タイプ名";
		rBSearchFamily.Text = "ファミリ名";
		buttonSearch.Text = "検索";
		buttonReset.Text = "リセット";
		searchContain = "いずれかを含む";
		searchEqual = "完全一致";
		searchStart = "前方一致";
		searchEnd = "後方一致";
		groupBoxExtended.Text = "拡張選択";
		groupBoxCondition.Text = "選定条件";
		checkBoxExCategory.Text = "同じカテゴリ";
		checkBoxExFamily.Text = "同じファミリ";
		checkBoxExType.Text = "同じタイプ";
		checkBoxExWorkset.Text = "同じワークセット";
		checkBoxExHostOfSel.Text = "選択した要素のホスト";
		checkBoxExNestOfSel.Text = "選択した要素はネスト";
		checkBoxExNested.Text = "ネストファミリ";
		checkBoxExHost.Text = "ホストファミリ";
		checkBoxExJoined.Text = "結合された要素";
		groupBoxSelectionRange.Text = "選択範囲";
		rBCurrentViewE.Text = "現在のビュー";
		rBAllProjectE.Text = "全てのプロジェクト";
		groupBoxSelMethod.Text = "選択方法";
		rBExtendSel.Text = "現在の選択を拡張する";
		rBNewSel.Text = "新選択";
		buttonExtend.Text = "選択を拡張する";
		buttonExcel.Text = "エクスポート";
	}

	private void ToolTips()
	{
		string message = "Check all";
		string message2 = "Uncheck all";
		string message3 = "Expand";
		string message4 = "Collapse";
		string message5 = "Select the checked elements in Revit";
		string message6 = "Exclude/deselect the checked elements from current selection in Revit";
		string message7 = "Close the form";
		string message8 = "Export all element info to excel";
		string message9 = "Show only selected elements";
		string message10 = "Show elements visible in the current view";
		string message11 = "Show all elements";
		string message12 = "Category ⇨ Family ⇨ Type ⇨ Element";
		string message13 = "Workset ⇨ Category ⇨ Family ⇨ Type ⇨ Element";
		string message14 = "Level ⇨ Category ⇨ Family ⇨ Type ⇨ Element";
		string message15 = "Username ⇨ Category ⇨ Family ⇨ Type ⇨ Element";
		string message16 = "Username ⇨ Category ⇨ Family ⇨ Type ⇨ Element";
		string message17 = "Username ⇨ Category ⇨ Family ⇨ Type ⇨ Element";
		string message18 = "Created Phase ⇨ Category ⇨ Family ⇨ Type ⇨ Element";
		string message19 = "Demolished Phase ⇨ Category ⇨ Family ⇨ Type ⇨ Element";
		string message20 = "Input text to search";
		string message21 = "Select a search option";
		string message22 = "Search in type names";
		string message23 = "Search in family names";
		string message24 = "Search and filter";
		string message25 = "Reset search filter";
		NewToolTip(buttonCheck, message);
		NewToolTip(buttonUncheck, message2);
		NewToolTip(buttonExpand, message3);
		NewToolTip(buttonCollapse, message4);
		NewToolTip(buttonSelect, message5);
		NewToolTip(buttonDeselect, message6);
		NewToolTip(buttonClose, message7);
		NewToolTip(buttonExcel, message8);
		NewToolTip(rBSelCurrent, message9);
		NewToolTip(rBSelView, message10);
		NewToolTip(rBSelAll, message11);
		NewToolTip(rBListType, message12);
		NewToolTip(rBListWorkset, message13);
		NewToolTip(rBListLevel, message14);
		NewToolTip(rBListCreated, message15);
		NewToolTip(rBListOwner, message16);
		NewToolTip(rBListChanged, message17);
		NewToolTip(rBListPhaseCreated, message18);
		NewToolTip(rBListPhaseDemolished, message19);
		NewToolTip(textBoxSearch, message20);
		NewToolTip(comboBoxSearch, message21);
		NewToolTip(rBSearchType, message22);
		NewToolTip(rBSearchFamily, message23);
		NewToolTip(buttonSearch, message24);
		NewToolTip(buttonReset, message25);
	}

	private ToolTip NewToolTip(System.Windows.Forms.Control control, string message)
	{
		ToolTip toolTip = new ToolTip();
		toolTip.SetToolTip(control, message);
		return toolTip;
	}

	private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
	{
		foreach (TreeNode node in treeNode.Nodes)
		{
			node.Checked = nodeChecked;
			if (node.Nodes.Count > 0)
			{
				CheckAllChildNodes(node, nodeChecked);
			}
		}
	}

	public void GetCheckedNodes(TreeNodeCollection nodes, List<TreeNode> treeNodes)
	{
		foreach (TreeNode node in nodes)
		{
			Console.WriteLine(node.Text);
			if (node.Nodes.Count != 0)
			{
				GetCheckedNodes(node.Nodes, treeNodes);
			}
			else if (node.Checked)
			{
				treeNodes.Add(node);
			}
		}
	}

	private void RefreshTreeView(List<Element> elements)
	{
		treeViewElements.Nodes.Clear();
		List<TreeNode> list = new List<TreeNode>();
		list = (rBListWorkset.Checked ? ElementsToTreeViewByWorkset(elements) : (rBListLevel.Checked ? ElementsToTreeViewByLevel(elements) : (rBListCreated.Checked ? ElementsToTreeViewByCreated(elements) : (rBListOwner.Checked ? ElementsToTreeViewByOwner(elements) : (rBListChanged.Checked ? ElementsToTreeViewByChanged(elements) : (rBListPhaseCreated.Checked ? ElementsToTreeViewByPhaseCreated(elements) : (rBListPhaseDemolished.Checked ? ElementsToTreeViewByPhaseDemolished(elements) : ((!rBListMaterial.Checked) ? ElementsToTreeViewByType(elements) : ElementsToTreeViewByMaterials(elements)))))))));
		TreeNodesToTreeView(list);
		labelCount.Text = textCount;
	}

	private void TreeNodesToTreeView(List<TreeNode> treeNodes)
	{
		TreeNode treeNode = new TreeNode();
		treeNode.Name = "nodeAll";
		treeNode.Text = "All Elements";
		foreach (TreeNode treeNode2 in treeNodes)
		{
			treeNode.Nodes.Add(treeNode2);
		}
		treeNode.Expand();
		treeViewElements.Nodes.Add(treeNode);
	}

	private List<TreeNode> ElementsToTreeViewByType(List<Element> elements)
	{
		List<TreeNode> list = new List<TreeNode>();
		Category categoryLine = Category.GetCategory(doc, BuiltInCategory.OST_Lines);
		List<List<Element>> list2 = (from x in elements
			where x.Category != null
			where x.Category.Id != categoryLine.Id
			group x by x.Category.Name into x
			orderby x.Key
			select x.ToList()).ToList();
		foreach (List<Element> item in list2)
		{
			try
			{
				TreeNode treeNode = new TreeNode("nodeElementCategory");
				List<Element> source = item.Where((Element x) => x.GetTypeId().IntegerValue != -1).ToList();
				List<Element> source2 = item.Where((Element x) => x.GetTypeId().IntegerValue == -1).ToList();
				if (source.Any())
				{
					List<List<Element>> list3 = (from x in source
						group x by (doc.GetElement(x.GetTypeId()) as ElementType).FamilyName into x
						orderby x.Key
						select x.ToList()).ToList();
					foreach (List<Element> item2 in list3)
					{
						List<List<Element>> list4 = (from x in item2
							group x by (doc.GetElement(x.GetTypeId()) as ElementType).Name into x
							orderby x.Key
							select x.ToList()).ToList();
						TreeNode treeNode2 = new TreeNode("nodeFamily");
						foreach (List<Element> item3 in list4)
						{
							TreeNode treeNode3 = new TreeNode("nodeSymbol");
							treeNode3.Text = doc.GetElement(item3.First().GetTypeId()).Name + ": " + item3.Count;
							if (checkBoxInstance.Checked)
							{
								foreach (Element item4 in item3)
								{
									TreeNode node = TreeNodeElement(item4);
									treeNode3.Nodes.Add(node);
								}
							}
							else
							{
								treeNode3.Tag = item3.Select((Element x) => x.Id).ToList();
							}
							treeNode2.Nodes.Add(treeNode3);
						}
						ElementType elementType = doc.GetElement(item2.First().GetTypeId()) as ElementType;
						treeNode2.Text = elementType.FamilyName + ": " + item2.Count;
						treeNode.Nodes.Add(treeNode2);
					}
				}
				if (source2.Any())
				{
					foreach (Element item5 in item)
					{
						TreeNode node2 = TreeNodeElement(item5);
						treeNode.Nodes.Add(node2);
					}
				}
				treeNode.Text = item.First().Category.Name + ": " + item.Count;
				list.Add(treeNode);
			}
			catch (Exception)
			{
			}
		}
		List<Element> list5 = (from x in elements
			where x.Category != null
			where x.Category.Id == categoryLine.Id
			select x).ToList();
		if (list5.Any())
		{
			TreeNode treeNode4 = new TreeNode("nodeLinesCategory");
			List<List<CurveElement>> list6 = (from CurveElement x in list5
				group x by x.CurveElementType into x
				orderby x.Key
				select x.ToList()).ToList();
			foreach (List<CurveElement> item6 in list6)
			{
				TreeNode treeNode5 = new TreeNode("nodeLineType");
				treeNode5.Text = item6.First().Name + ": " + item6.Count;
				List<List<CurveElement>> list7 = (from x in item6
					group x by x.LineStyle.Name into x
					orderby x.Key
					select x.ToList()).ToList();
				foreach (List<CurveElement> item7 in list7)
				{
					TreeNode treeNode6 = new TreeNode("nodeLineStyle");
					treeNode6.Text = item7.First().LineStyle.Name + ": " + item7.Count;
					foreach (CurveElement item8 in item7)
					{
						TreeNode node3 = TreeNodeElement(item8);
						treeNode6.Nodes.Add(node3);
					}
					treeNode5.Nodes.Add(treeNode6);
				}
				treeNode4.Nodes.Add(treeNode5);
			}
			treeNode4.Text = categoryLine.Name + ": " + list5.Count;
			list.Add(treeNode4);
		}
		return list;
	}

	private List<TreeNode> ElementsToTreeViewByTypeBackupForOpt(List<Element> elements)
	{
		List<TreeNode> list = new List<TreeNode>();
		Category categoryLine = Category.GetCategory(doc, BuiltInCategory.OST_Lines);
		List<List<Element>> list2 = (from x in elements
			where x.Category != null
			where x.Category.Id != categoryLine.Id
			group x by x.Category.Name into x
			orderby x.Key
			select x.ToList()).ToList();
		foreach (List<Element> item in list2)
		{
			try
			{
				TreeNode treeNode = new TreeNode();
				treeNode.Name = "nodeElementCategory";
				List<Element> list3 = new List<Element>();
				List<Element> list4 = new List<Element>();
				foreach (Element item2 in item)
				{
					try
					{
						if (item2.GetTypeId().IntegerValue == -1)
						{
							list4.Add(item2);
						}
						else
						{
							list3.Add(item2);
						}
					}
					catch (Exception)
					{
					}
				}
				if (list3.Any())
				{
					List<List<Element>> list5 = (from x in list3
						group x by (doc.GetElement(x.GetTypeId()) as ElementType).FamilyName into x
						orderby x.Key
						select x.ToList()).ToList();
					foreach (List<Element> item3 in list5)
					{
						List<List<Element>> list6 = (from x in item3
							group x by (doc.GetElement(x.GetTypeId()) as ElementType).Name into x
							orderby x.Key
							select x.ToList()).ToList();
						TreeNode treeNode2 = new TreeNode();
						treeNode2.Name = "nodeFamily";
						foreach (List<Element> item4 in list6)
						{
							TreeNode treeNode3 = new TreeNode();
							treeNode3.Name = "nodeSymbol";
							treeNode3.Text = doc.GetElement(item4.First().GetTypeId()).Name + ": " + item4.Count;
							if (checkBoxInstance.Checked)
							{
								foreach (Element item5 in item4)
								{
									try
									{
										TreeNode node = TreeNodeElement(item5);
										treeNode3.Nodes.Add(node);
									}
									catch (Exception)
									{
									}
								}
							}
							else
							{
								treeNode3.Tag = item4.Select((Element x) => x.Id).ToList();
							}
							treeNode2.Nodes.Add(treeNode3);
						}
						ElementType elementType = doc.GetElement(item3.First().GetTypeId()) as ElementType;
						treeNode2.Text = elementType.FamilyName + ": " + item3.Count;
						treeNode.Nodes.Add(treeNode2);
					}
				}
				if (list4.Any())
				{
					foreach (Element item6 in item)
					{
						TreeNode node2 = TreeNodeElement(item6);
						treeNode.Nodes.Add(node2);
					}
				}
				treeNode.Text = item.First().Category.Name + ": " + item.Count;
				list.Add(treeNode);
			}
			catch (Exception)
			{
			}
		}
		List<Element> list7 = (from x in elements
			where x.Category != null
			where x.Category.Id == categoryLine.Id
			select x).ToList();
		if (list7.Any())
		{
			TreeNode treeNode4 = new TreeNode();
			treeNode4.Name = "nodeElementCategory";
			List<List<CurveElement>> list8 = (from CurveElement x in list7
				group x by x.CurveElementType into x
				orderby x.Key
				select x.ToList()).ToList();
			foreach (List<CurveElement> item7 in list8)
			{
				TreeNode treeNode5 = new TreeNode();
				treeNode5.Name = "nodeType";
				treeNode5.Text = item7.First().Name + ": " + item7.Count;
				List<List<CurveElement>> list9 = (from x in item7
					group x by x.LineStyle.Name into x
					orderby x.Key
					select x.ToList()).ToList();
				foreach (List<CurveElement> item8 in list9)
				{
					TreeNode treeNode6 = new TreeNode();
					treeNode6.Name = "nodeLineStyle";
					treeNode6.Text = item8.First().LineStyle.Name + ": " + item8.Count;
					foreach (CurveElement item9 in item8)
					{
						TreeNode node3 = TreeNodeElement(item9);
						treeNode6.Nodes.Add(node3);
					}
					treeNode5.Nodes.Add(treeNode6);
				}
				treeNode4.Nodes.Add(treeNode5);
			}
			treeNode4.Text = categoryLine.Name + ": " + list7.Count;
			list.Add(treeNode4);
		}
		return list;
	}

	private List<TreeNode> ElementsToTreeViewByWorkset(List<Element> elements)
	{
		List<List<Element>> list = (from x in elements
			where x.WorksetId != null
			group x by x.WorksetId into x
			select x.ToList()).ToList();
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (List<Element> item in list)
		{
			Workset workset = doc.GetWorksetTable().GetWorkset(item.First().WorksetId);
			WorksetKind kind = workset.Kind;
			if (kind != WorksetKind.UserWorkset)
			{
				continue;
			}
			TreeNode treeNode = new TreeNode();
			List<TreeNode> list3 = ElementsToTreeViewByType(item);
			foreach (TreeNode item2 in list3)
			{
				treeNode.Nodes.Add(item2);
			}
			treeNode.Text = workset.Name + ": " + item.Count;
			list2.Add(treeNode);
		}
		return list2;
	}

	private List<TreeNode> ElementsToTreeViewByCreated(List<Element> elements)
	{
		List<List<Element>> list = (from x in elements
			group x by WorksharingUtils.GetWorksharingTooltipInfo(doc, x.Id).Creator into x
			orderby x.Key
			select x.ToList()).ToList();
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (List<Element> item in list)
		{
			TreeNode treeNode = new TreeNode();
			List<TreeNode> list3 = ElementsToTreeViewByType(item);
			foreach (TreeNode item2 in list3)
			{
				treeNode.Nodes.Add(item2);
			}
			WorksharingTooltipInfo worksharingTooltipInfo = WorksharingUtils.GetWorksharingTooltipInfo(doc, item.First().Id);
			treeNode.Text = worksharingTooltipInfo.Creator + ": " + item.Count;
			list2.Add(treeNode);
		}
		return list2;
	}

	private List<TreeNode> ElementsToTreeViewByOwner(List<Element> elements)
	{
		List<List<Element>> list = (from x in elements
			group x by WorksharingUtils.GetWorksharingTooltipInfo(doc, x.Id).Owner into x
			orderby x.Key
			select x.ToList()).ToList();
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (List<Element> item in list)
		{
			TreeNode treeNode = new TreeNode();
			List<TreeNode> list3 = ElementsToTreeViewByType(item);
			foreach (TreeNode item2 in list3)
			{
				treeNode.Nodes.Add(item2);
			}
			WorksharingTooltipInfo worksharingTooltipInfo = WorksharingUtils.GetWorksharingTooltipInfo(doc, item.First().Id);
			treeNode.Text = worksharingTooltipInfo.Owner + ": " + item.Count;
			list2.Add(treeNode);
		}
		return list2;
	}

	private List<TreeNode> ElementsToTreeViewByChanged(List<Element> elements)
	{
		List<List<Element>> list = (from x in elements
			group x by WorksharingUtils.GetWorksharingTooltipInfo(doc, x.Id).LastChangedBy into x
			orderby x.Key
			select x.ToList()).ToList();
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (List<Element> item in list)
		{
			TreeNode treeNode = new TreeNode();
			List<TreeNode> list3 = ElementsToTreeViewByType(item);
			foreach (TreeNode item2 in list3)
			{
				treeNode.Nodes.Add(item2);
			}
			WorksharingTooltipInfo worksharingTooltipInfo = WorksharingUtils.GetWorksharingTooltipInfo(doc, item.First().Id);
			treeNode.Text = worksharingTooltipInfo.LastChangedBy + ": " + item.Count;
			list2.Add(treeNode);
		}
		return list2;
	}

	private List<TreeNode> ElementsToTreeViewByLevel(List<Element> elements)
	{
		FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
		IEnumerable<Level> enumerable = (from Level lvl in filteredElementCollector.OfClass(typeof(Level))
			orderby lvl.Elevation
			select lvl).ToList();
		List<Element> list = new List<Element>();
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (Level item in enumerable)
		{
			TreeNode treeNode = new TreeNode();
			List<Element> list3 = new List<Element>();
			foreach (Element element in elements)
			{
				try
				{
					if (element.LevelId == null || element.LevelId.IntegerValue == -1)
					{
						Parameter parameter = element.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);
						if (parameter != null)
						{
							if (parameter.AsElementId() == item.Id)
							{
								list3.Add(element);
							}
							continue;
						}
						Parameter parameter2 = element.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM);
						if (parameter2 != null && parameter2.AsElementId() == item.Id)
						{
							list3.Add(element);
						}
						else
						{
							list.Add(element);
						}
					}
					else if (element.LevelId == item.Id)
					{
						list3.Add(element);
					}
				}
				catch (Exception)
				{
				}
			}
			if (!list3.Any())
			{
				continue;
			}
			List<TreeNode> list4 = ElementsToTreeViewByType(list3);
			foreach (TreeNode item2 in list4)
			{
				treeNode.Nodes.Add(item2);
			}
			treeNode.Text = item.Name + ": " + list3.Count;
			list2.Add(treeNode);
		}
		if (list.Any())
		{
			TreeNode treeNode2 = new TreeNode();
			List<TreeNode> list5 = ElementsToTreeViewByType(list);
			foreach (TreeNode item3 in list5)
			{
				treeNode2.Nodes.Add(item3);
			}
			treeNode2.Text = "No Level: " + list.Count;
			list2.Add(treeNode2);
		}
		return list2;
	}

	private List<TreeNode> ElementsToTreeViewByPhaseCreated(List<Element> elements)
	{
		List<List<Element>> list = (from x in elements
			where x.HasPhases()
			group x by x.CreatedPhaseId into x
			select x.ToList()).ToList();
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (List<Element> item in list)
		{
			ElementId createdPhaseId = item.First().CreatedPhaseId;
			string text = "None";
			if (createdPhaseId != ElementId.InvalidElementId)
			{
				text = doc.GetElement(createdPhaseId).Name;
			}
			TreeNode treeNode = new TreeNode();
			List<TreeNode> list3 = ElementsToTreeViewByType(item);
			foreach (TreeNode item2 in list3)
			{
				treeNode.Nodes.Add(item2);
			}
			treeNode.Text = text + ": " + item.Count;
			list2.Add(treeNode);
		}
		return list2;
	}

	private List<TreeNode> ElementsToTreeViewByPhaseDemolished(List<Element> elements)
	{
		List<List<Element>> list = (from x in elements
			where x.HasPhases()
			group x by x.DemolishedPhaseId into x
			select x.ToList()).ToList();
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (List<Element> item in list)
		{
			ElementId demolishedPhaseId = item.First().DemolishedPhaseId;
			string text = "None";
			if (demolishedPhaseId != ElementId.InvalidElementId)
			{
				text = doc.GetElement(demolishedPhaseId).Name;
			}
			TreeNode treeNode = new TreeNode();
			List<TreeNode> list3 = ElementsToTreeViewByType(item);
			foreach (TreeNode item2 in list3)
			{
				treeNode.Nodes.Add(item2);
			}
			treeNode.Text = text + ": " + item.Count;
			list2.Add(treeNode);
		}
		return list2;
	}

	private List<TreeNode> ElementsToTreeViewByMaterials(List<Element> elements)
	{
		List<TreeNode> list = new List<TreeNode>();
		List<Material> list2 = (from Material x in new FilteredElementCollector(doc).OfClass(typeof(Material))
			orderby x.Name
			select x).ToList();
		foreach (Material item in list2)
		{
			List<Element> list3 = new List<Element>();
			foreach (Element element in elements)
			{
				foreach (ElementId materialId in element.GetMaterialIds(returnPaintMaterials: false))
				{
					if (materialId == item.Id)
					{
						list3.Add(element);
						break;
					}
				}
			}
			if (!list3.Any())
			{
				continue;
			}
			TreeNode treeNode = new TreeNode
			{
				Name = item.Name,
				Text = item.Name
			};
			List<TreeNode> list4 = ElementsToTreeViewByType(list3);
			foreach (TreeNode item2 in list4)
			{
				treeNode.Nodes.Add(item2);
			}
			list.Add(treeNode);
		}
		return list;
	}

	private TreeNode NestedToTreeNode(Element element)
	{
		TreeNode result = TreeNodeElement(element);
		FamilyInstance familyInstance = element as FamilyInstance;
		ICollection<ElementId> subComponentIds = familyInstance.GetSubComponentIds();
		if (subComponentIds.Count > 0)
		{
			foreach (ElementId item in subComponentIds)
			{
				try
				{
					Element element2 = view.Document.GetElement(item);
					NestedToTreeNode(element2);
				}
				catch (Exception)
				{
				}
			}
		}
		return result;
	}

	private List<Element> GetElementsForTreeView()
	{
		Document document = uidoc.Document;
		Autodesk.Revit.DB.View activeView = uidoc.ActiveView;
		List<Element> list = new List<Element>();
		if (rBSelAll.Checked)
		{
			list = new FilteredElementCollector(document).WhereElementIsNotElementType().ToList();
		}
		else if (rBSelView.Checked)
		{
			list = new FilteredElementCollector(document, activeView.Id).WhereElementIsNotElementType().ToList();
		}
		else if (rBSelCurrent.Checked)
		{
			Selection selection = uidoc.Selection;
			ICollection<ElementId> elementIds = selection.GetElementIds();
			foreach (ElementId item in elementIds)
			{
				try
				{
					Element element = document.GetElement(item);
					list.Add(element);
				}
				catch (Exception)
				{
				}
			}
		}
		if (checkBoxNested.Checked)
		{
			list = FilterNested(list);
		}
		if (checkBoxHost.Checked)
		{
			list = FilterHost(list);
		}
		if (checkBoxInplace.Checked)
		{
			list = FilterInplace(list);
		}
		if (checkBoxWarnings.Checked)
		{
			list = FilterWarnings(list);
		}
		return list;
	}

	private List<Element> FilterNested(List<Element> elements)
	{
		List<Element> list = new List<Element>();
		foreach (Element element in elements)
		{
			try
			{
				if (element is FamilyInstance)
				{
					FamilyInstance familyInstance = element as FamilyInstance;
					Element superComponent = familyInstance.SuperComponent;
					if (superComponent != null)
					{
						list.Add(element);
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return list;
	}

	private List<Element> FilterHost(List<Element> elements)
	{
		List<Element> list = new List<Element>();
		foreach (Element element in elements)
		{
			try
			{
				if (element is FamilyInstance)
				{
					FamilyInstance familyInstance = element as FamilyInstance;
					ICollection<ElementId> subComponentIds = familyInstance.GetSubComponentIds();
					if (subComponentIds.Count > 0)
					{
						list.Add(element);
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return list;
	}

	private List<Element> FilterInplace(List<Element> elements)
	{
		List<Element> list = new List<Element>();
		foreach (Element element in elements)
		{
			try
			{
				if (element is FamilyInstance)
				{
					FamilyInstance familyInstance = element as FamilyInstance;
					Family family = familyInstance.Symbol.Family;
					if (family.IsInPlace)
					{
						list.Add(element);
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return list;
	}

	private List<Element> FilterWarnings(List<Element> elements)
	{
		List<Element> list = new List<Element>();
		IList<FailureMessage> warnings = doc.GetWarnings();
		HashSet<ElementId> hashSet = new HashSet<ElementId>();
		List<Element> list2 = new List<Element>();
		foreach (FailureMessage item in warnings)
		{
			foreach (ElementId failingElement in item.GetFailingElements())
			{
				hashSet.Add(failingElement);
			}
		}
		foreach (ElementId item2 in hashSet)
		{
			list2.Add(doc.GetElement(item2));
		}
		foreach (Element element in elements)
		{
			try
			{
				if (hashSet.Contains(element.Id))
				{
					list.Add(element);
				}
			}
			catch (Exception)
			{
			}
		}
		return list;
	}

	private List<Element> ElementsSearch(List<Element> elements, string text)
	{
		List<Element> list = new List<Element>();
		if (rBSearchType.Checked)
		{
			if (comboBoxSearch.SelectedIndex == 0)
			{
				list = elements.Where((Element x) => x.Name.Contains(text)).ToList();
			}
			else if (comboBoxSearch.SelectedIndex == 1)
			{
				list = elements.Where((Element x) => x.Name.Equals(text)).ToList();
			}
			else if (comboBoxSearch.SelectedIndex == 2)
			{
				list = elements.Where((Element x) => x.Name.StartsWith(text)).ToList();
			}
			else if (comboBoxSearch.SelectedIndex == 3)
			{
				list = elements.Where((Element x) => x.Name.EndsWith(text)).ToList();
			}
		}
		else
		{
			foreach (Element element in elements)
			{
				if (element is FamilyInstance)
				{
					FamilyInstance familyInstance = element as FamilyInstance;
					string familyName = familyInstance.Symbol.FamilyName;
					if (comboBoxSearch.SelectedIndex == 0 && familyName.Contains(text))
					{
						list.Add(element);
					}
					else if (comboBoxSearch.SelectedIndex == 1 && familyName.Equals(text))
					{
						list.Add(element);
					}
					else if (comboBoxSearch.SelectedIndex == 2 && familyName.StartsWith(text))
					{
						list.Add(element);
					}
					else if (comboBoxSearch.SelectedIndex == 3 && familyName.EndsWith(text))
					{
						list.Add(element);
					}
				}
			}
		}
		return list;
	}

	public void CheckAllNodes(TreeNodeCollection nodes)
	{
		foreach (TreeNode node in nodes)
		{
			node.Checked = true;
			CheckChildren(node, isChecked: true);
		}
	}

	public void UncheckAllNodes(TreeNodeCollection nodes)
	{
		foreach (TreeNode node in nodes)
		{
			node.Checked = false;
			CheckChildren(node, isChecked: false);
		}
	}

	private void CheckChildren(TreeNode rootNode, bool isChecked)
	{
		foreach (TreeNode node in rootNode.Nodes)
		{
			CheckChildren(node, isChecked);
			node.Checked = isChecked;
		}
	}

	private TreeNode TreeNodeElement(Element element, bool boolElementId = false)
	{
		TreeNode treeNode = new TreeNode();
		treeNode.Name = "nodeInstance";
		if (boolElementId)
		{
			treeNode.Text = element.Name + " (" + element.Id?.ToString() + ")";
		}
		else
		{
			treeNode.Text = element.Name;
		}
		treeNode.Tag = new List<ElementId> { element.Id };
		return treeNode;
	}

	private void RecurseNodes(TreeNodeCollection currentNode, Worksheet worksheet, int row, int col)
	{
		foreach (TreeNode item in currentNode)
		{
			row++;
			((dynamic)worksheet.Cells[row, col]).Value = item.Text;
			if (item.FirstNode != null)
			{
				RecurseNodes(item.Nodes, worksheet, row, col + 1);
			}
		}
	}

	private void ExportTreeNode(Document doc, Worksheet worksheet, TreeNodeCollection treeNodeCol, int col)
	{
		foreach (TreeNode item in treeNodeCol)
		{
			try
			{
				worksheet.Cells[excelRow, col] = item.Text;
				TreeNodeCollection nodes = item.Nodes;
				if (item.FirstNode != null)
				{
					int num = excelRow;
					ExportTreeNode(doc, worksheet, nodes, col + 1);
					Range range = worksheet.Range[(dynamic)worksheet.Cells[num, col], (dynamic)worksheet.Cells[excelRow - 1, col]];
					range.Merge(Type.Missing);
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
				}
				else
				{
					excelRow++;
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
	{
	}

	private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
	{
		if (e.Action != 0 && e.Node.Nodes.Count > 0)
		{
			CheckAllChildNodes(e.Node, e.Node.Checked);
		}
		List<TreeNode> list = new List<TreeNode>();
		GetCheckedNodes(treeViewElements.Nodes, list);
		labelCount.Text = textCount + list.Count;
	}

	private void radioButtonCurrentSel_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void radioButtonCurrentView_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void radioButtonAllProject_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void radioButtonFamilyType_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void radioButtonWorkset_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void radioButtonUser_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void radioButtonListLevel_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void rButtonListOwner_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void rButtonListChanged_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void radioButtonPhase_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void rBListMaterial_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void rBListSign_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void buttonSelect_Click(object sender, EventArgs e)
	{
		List<TreeNode> list = new List<TreeNode>();
		GetCheckedNodes(treeViewElements.Nodes, list);
		checkedIds = new List<ElementId>();
		foreach (TreeNode item in list)
		{
			List<ElementId> collection = item.Tag as List<ElementId>;
			checkedIds.AddRange(collection);
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void buttonSearch_Click(object sender, EventArgs e)
	{
		string text = textBoxSearch.Text;
		RefreshTreeView(ElementsSearch(GetElementsForTreeView(), text));
	}

	private void buttonReset_Click(object sender, EventArgs e)
	{
		RefreshTreeView(GetElementsForTreeView());
	}

	//private void buttonExcel_Click(object sender, EventArgs e)
	//{
	//	Worksheet worksheet = Utils.ExcelWorksheet("TreeView Export");
	//	excelRow = 1;
	//	int num = 0;
	//	foreach (TreeNode node in treeViewElements.Nodes)
	//	{
	//		ExportTreeNode(doc, worksheet, treeViewElements.Nodes, ++num);
	//	}
	//	Utils.ExcelTitleStyle(worksheet);
	//}

	private void buttonCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void buttonCheck_Click(object sender, EventArgs e)
	{
		CheckAllNodes(treeViewElements.Nodes);
	}

	private void buttonUncheck_Click(object sender, EventArgs e)
	{
		UncheckAllNodes(treeViewElements.Nodes);
	}

	private void buttonDeselect_Click(object sender, EventArgs e)
	{
		uidoc.Selection.SetElementIds(new List<ElementId>());
	}

	private void buttonExtend_Click(object sender, EventArgs e)
	{
		Selection selection = uidoc.Selection;
		ICollection<ElementId> elementIds = selection.GetElementIds();
		if (!elementIds.Any())
		{
			return;
		}
		Element element = doc.GetElement(elementIds.First());
		FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc, view.Id);
		if (rBAllProjectE.Checked)
		{
			filteredElementCollector = new FilteredElementCollector(doc);
		}
		if (checkBoxExCategory.Checked)
		{
			ElementCategoryFilter filter = new ElementCategoryFilter(element.Category.Id);
			filteredElementCollector.WherePasses(filter);
		}
		if (checkBoxExWorkset.Checked)
		{
			ElementWorksetFilter filter2 = new ElementWorksetFilter(element.WorksetId);
			filteredElementCollector.WherePasses(filter2);
		}
		List<Element> list = filteredElementCollector.WhereElementIsNotElementType().ToList();
		if (checkBoxExFamily.Checked)
		{
			ElementType elementType = doc.GetElement(element.GetTypeId()) as ElementType;
			string familyName = elementType.FamilyName;
			List<Element> list2 = new List<Element>();
			foreach (Element item in list)
			{
				if (doc.GetElement(item.GetTypeId()) is ElementType { FamilyName: var familyName2 } && familyName2 == familyName)
				{
					list2.Add(item);
				}
			}
			list = list2;
		}
		if (checkBoxExType.Checked)
		{
			list = list.Where((Element x) => x.GetTypeId().Equals(element.GetTypeId())).ToList();
		}
		if (checkBoxExJoined.Checked)
		{
			ICollection<ElementId> joinedElements = JoinGeometryUtils.GetJoinedElements(doc, element);
			List<Element> list3 = new List<Element>();
			foreach (ElementId item2 in joinedElements)
			{
				Element element2 = doc.GetElement(item2);
				list3.Add(element2);
			}
			list = list3;
		}
		if (checkBoxExHostOfSel.Checked && element is FamilyInstance)
		{
			FamilyInstance familyInstance = element as FamilyInstance;
			Element host = familyInstance.Host;
			list = ((host == null) ? new List<Element>() : new List<Element> { host });
		}
		if (checkBoxExNestOfSel.Checked)
		{
			IList<ElementId> dependentElements = element.GetDependentElements(null);
			List<Element> list4 = new List<Element>();
			foreach (ElementId item3 in dependentElements)
			{
				try
				{
					foreach (Element item4 in list)
					{
						if (item3 == item4.Id)
						{
							Element element3 = doc.GetElement(item3);
							list4.Add(element3);
							break;
						}
					}
				}
				catch (Exception)
				{
				}
			}
			list = list4;
		}
		if (checkBoxExHost.Checked)
		{
			list = FilterHost(list);
		}
		if (checkBoxExNested.Checked)
		{
			list = FilterNested(list);
		}
		if (rBExtendSel.Checked)
		{
			list.Add(element);
		}
		treeViewElements.Nodes.Clear();
		List<TreeNode> treeNodes = ElementsToTreeViewByType(list);
		TreeNodesToTreeView(treeNodes);
	}

	private void buttonExpand_Click(object sender, EventArgs e)
	{
		treeViewElements.ExpandAll();
	}

	private void buttonCollapse_Click(object sender, EventArgs e)
	{
		treeViewElements.CollapseAll();
	}

	private void buttonRemove_Click(object sender, EventArgs e)
	{
		List<TreeNode> list = new List<TreeNode>();
		GetCheckedNodes(treeViewElements.Nodes, list);
		foreach (TreeNode item in list)
		{
			item.Remove();
		}
	}

	private void buttonRefresh_Click(object sender, EventArgs e)
	{
		RefreshTreeView(GetElementsForTreeView());
	}

	private void buttonAdvanced_Click(object sender, EventArgs e)
	{
		int left = treeViewElements.Left;
		if (advanced)
		{
			Size size = base.Size;
			base.Size = new Size(groupBoxExtended.Left + left, size.Height);
			advanced = false;
			buttonAdvanced.Text = ">>";
		}
		else
		{
			Size size2 = base.Size;
			base.Size = new Size(groupBoxExtended.Right + 2 * left, size2.Height);
			advanced = true;
			buttonAdvanced.Text = "<<";
		}
	}

	private void checkBoxInstance_CheckedChanged(object sender, EventArgs e)
	{
		if (formLoad)
		{
			RefreshTreeView(GetElementsForTreeView());
		}
	}

	private void checkBoxInplace_CheckedChanged(object sender, EventArgs e)
	{
		RefreshTreeView(GetElementsForTreeView());
	}

	private void checkBoxWarnings_CheckedChanged(object sender, EventArgs e)
	{
		RefreshTreeView(GetElementsForTreeView());
	}

	private void checkBoxNested_CheckedChanged(object sender, EventArgs e)
	{
		RefreshTreeView(GetElementsForTreeView());
	}

	private void checkBoxHost_CheckedChanged(object sender, EventArgs e)
	{
		RefreshTreeView(GetElementsForTreeView());
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ONES.SelectFilterForm));
		this.treeViewElements = new System.Windows.Forms.TreeView();
		this.labelCount = new System.Windows.Forms.Label();
		this.groupBoxSelection = new System.Windows.Forms.GroupBox();
		this.rBSelAll = new System.Windows.Forms.RadioButton();
		this.rBSelCurrent = new System.Windows.Forms.RadioButton();
		this.rBSelView = new System.Windows.Forms.RadioButton();
		this.buttonSelect = new System.Windows.Forms.Button();
		this.groupBoxCondition = new System.Windows.Forms.GroupBox();
		this.checkBoxExJoined = new System.Windows.Forms.CheckBox();
		this.checkBoxExNested = new System.Windows.Forms.CheckBox();
		this.checkBoxExNestOfSel = new System.Windows.Forms.CheckBox();
		this.checkBoxExHostOfSel = new System.Windows.Forms.CheckBox();
		this.checkBoxExHost = new System.Windows.Forms.CheckBox();
		this.checkBoxExWorkset = new System.Windows.Forms.CheckBox();
		this.checkBoxExFamily = new System.Windows.Forms.CheckBox();
		this.checkBoxExType = new System.Windows.Forms.CheckBox();
		this.checkBoxExCategory = new System.Windows.Forms.CheckBox();
		this.checkBoxHost = new System.Windows.Forms.CheckBox();
		this.checkBoxNested = new System.Windows.Forms.CheckBox();
		this.buttonDeselect = new System.Windows.Forms.Button();
		this.groupBoxExtended = new System.Windows.Forms.GroupBox();
		this.buttonExtend = new System.Windows.Forms.Button();
		this.groupBoxSelMethod = new System.Windows.Forms.GroupBox();
		this.rBNewSel = new System.Windows.Forms.RadioButton();
		this.rBExtendSel = new System.Windows.Forms.RadioButton();
		this.groupBoxSelectionRange = new System.Windows.Forms.GroupBox();
		this.rBAllProjectE = new System.Windows.Forms.RadioButton();
		this.rBCurrentViewE = new System.Windows.Forms.RadioButton();
		this.groupBoxListingOption = new System.Windows.Forms.GroupBox();
		this.rBListMaterial = new System.Windows.Forms.RadioButton();
		this.rBListPhaseDemolished = new System.Windows.Forms.RadioButton();
		this.rBListPhaseCreated = new System.Windows.Forms.RadioButton();
		this.rBListChanged = new System.Windows.Forms.RadioButton();
		this.rBListOwner = new System.Windows.Forms.RadioButton();
		this.rBListLevel = new System.Windows.Forms.RadioButton();
		this.rBListType = new System.Windows.Forms.RadioButton();
		this.rBListCreated = new System.Windows.Forms.RadioButton();
		this.rBListWorkset = new System.Windows.Forms.RadioButton();
		this.checkBoxInstance = new System.Windows.Forms.CheckBox();
		this.groupBoxSearch = new System.Windows.Forms.GroupBox();
		this.rBSearchFamily = new System.Windows.Forms.RadioButton();
		this.rBSearchType = new System.Windows.Forms.RadioButton();
		this.comboBoxSearch = new System.Windows.Forms.ComboBox();
		this.buttonReset = new System.Windows.Forms.Button();
		this.buttonSearch = new System.Windows.Forms.Button();
		this.textBoxSearch = new System.Windows.Forms.TextBox();
		this.buttonExcel = new System.Windows.Forms.Button();
		this.buttonClose = new System.Windows.Forms.Button();
		this.groupBoxFiltering = new System.Windows.Forms.GroupBox();
		this.checkBoxWarnings = new System.Windows.Forms.CheckBox();
		this.checkBoxInplace = new System.Windows.Forms.CheckBox();
		this.buttonRefresh = new System.Windows.Forms.Button();
		this.buttonAdvanced = new System.Windows.Forms.Button();
		this.buttonCollapse = new System.Windows.Forms.Button();
		this.buttonExpand = new System.Windows.Forms.Button();
		this.buttonUncheck = new System.Windows.Forms.Button();
		this.buttonCheck = new System.Windows.Forms.Button();
		this.buttonRemove = new System.Windows.Forms.Button();
		this.groupBoxSelection.SuspendLayout();
		this.groupBoxCondition.SuspendLayout();
		this.groupBoxExtended.SuspendLayout();
		this.groupBoxSelMethod.SuspendLayout();
		this.groupBoxSelectionRange.SuspendLayout();
		this.groupBoxListingOption.SuspendLayout();
		this.groupBoxSearch.SuspendLayout();
		this.groupBoxFiltering.SuspendLayout();
		base.SuspendLayout();
		this.treeViewElements.CheckBoxes = true;
		this.treeViewElements.Location = new System.Drawing.Point(12, 40);
		this.treeViewElements.Name = "treeViewElements";
		this.treeViewElements.Size = new System.Drawing.Size(400, 607);
		this.treeViewElements.TabIndex = 0;
		this.treeViewElements.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(treeView1_AfterCheck);
		this.treeViewElements.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(treeView1_AfterSelect);
		this.labelCount.AutoSize = true;
		this.labelCount.Font = new System.Drawing.Font("MS UI Gothic", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
		this.labelCount.Location = new System.Drawing.Point(427, 560);
		this.labelCount.Name = "labelCount";
		this.labelCount.Size = new System.Drawing.Size(112, 16);
		this.labelCount.TabIndex = 1;
		this.labelCount.Text = "Total Selection:";
		this.groupBoxSelection.Controls.Add(this.rBSelAll);
		this.groupBoxSelection.Controls.Add(this.rBSelCurrent);
		this.groupBoxSelection.Controls.Add(this.rBSelView);
		this.groupBoxSelection.Location = new System.Drawing.Point(418, 12);
		this.groupBoxSelection.Name = "groupBoxSelection";
		this.groupBoxSelection.Size = new System.Drawing.Size(194, 85);
		this.groupBoxSelection.TabIndex = 2;
		this.groupBoxSelection.TabStop = false;
		this.groupBoxSelection.Text = "Selection Range";
		this.rBSelAll.AutoSize = true;
		this.rBSelAll.Location = new System.Drawing.Point(6, 62);
		this.rBSelAll.Name = "rBSelAll";
		this.rBSelAll.Size = new System.Drawing.Size(77, 16);
		this.rBSelAll.TabIndex = 3;
		this.rBSelAll.TabStop = true;
		this.rBSelAll.Text = "All Project";
		this.rBSelAll.UseVisualStyleBackColor = true;
		this.rBSelAll.CheckedChanged += new System.EventHandler(radioButtonAllProject_CheckedChanged);
		this.rBSelCurrent.AutoSize = true;
		this.rBSelCurrent.Location = new System.Drawing.Point(7, 18);
		this.rBSelCurrent.Name = "rBSelCurrent";
		this.rBSelCurrent.Size = new System.Drawing.Size(112, 16);
		this.rBSelCurrent.TabIndex = 3;
		this.rBSelCurrent.TabStop = true;
		this.rBSelCurrent.Text = "Current Selection";
		this.rBSelCurrent.UseVisualStyleBackColor = true;
		this.rBSelCurrent.CheckedChanged += new System.EventHandler(radioButtonCurrentSel_CheckedChanged);
		this.rBSelView.AutoSize = true;
		this.rBSelView.Location = new System.Drawing.Point(6, 40);
		this.rBSelView.Name = "rBSelView";
		this.rBSelView.Size = new System.Drawing.Size(90, 16);
		this.rBSelView.TabIndex = 3;
		this.rBSelView.TabStop = true;
		this.rBSelView.Text = "Current View";
		this.rBSelView.UseVisualStyleBackColor = true;
		this.rBSelView.CheckedChanged += new System.EventHandler(radioButtonCurrentView_CheckedChanged);
		this.buttonSelect.BackColor = System.Drawing.SystemColors.ActiveCaption;
		this.buttonSelect.Location = new System.Drawing.Point(430, 579);
		this.buttonSelect.Name = "buttonSelect";
		this.buttonSelect.Size = new System.Drawing.Size(75, 30);
		this.buttonSelect.TabIndex = 3;
		this.buttonSelect.Text = "Select";
		this.buttonSelect.UseVisualStyleBackColor = false;
		this.buttonSelect.Click += new System.EventHandler(buttonSelect_Click);
		this.groupBoxCondition.Controls.Add(this.checkBoxExJoined);
		this.groupBoxCondition.Controls.Add(this.checkBoxExNested);
		this.groupBoxCondition.Controls.Add(this.checkBoxExNestOfSel);
		this.groupBoxCondition.Controls.Add(this.checkBoxExHostOfSel);
		this.groupBoxCondition.Controls.Add(this.checkBoxExHost);
		this.groupBoxCondition.Controls.Add(this.checkBoxExWorkset);
		this.groupBoxCondition.Controls.Add(this.checkBoxExFamily);
		this.groupBoxCondition.Controls.Add(this.checkBoxExType);
		this.groupBoxCondition.Controls.Add(this.checkBoxExCategory);
		this.groupBoxCondition.Location = new System.Drawing.Point(9, 86);
		this.groupBoxCondition.Name = "groupBoxCondition";
		this.groupBoxCondition.Size = new System.Drawing.Size(189, 212);
		this.groupBoxCondition.TabIndex = 4;
		this.groupBoxCondition.TabStop = false;
		this.groupBoxCondition.Text = "Selection Condition";
		this.checkBoxExJoined.AutoSize = true;
		this.checkBoxExJoined.Location = new System.Drawing.Point(6, 150);
		this.checkBoxExJoined.Name = "checkBoxExJoined";
		this.checkBoxExJoined.Size = new System.Drawing.Size(109, 16);
		this.checkBoxExJoined.TabIndex = 0;
		this.checkBoxExJoined.Text = "Joined Elements";
		this.checkBoxExJoined.UseVisualStyleBackColor = true;
		this.checkBoxExNested.AutoSize = true;
		this.checkBoxExNested.Location = new System.Drawing.Point(6, 171);
		this.checkBoxExNested.Name = "checkBoxExNested";
		this.checkBoxExNested.Size = new System.Drawing.Size(98, 16);
		this.checkBoxExNested.TabIndex = 0;
		this.checkBoxExNested.Text = "Nested Family";
		this.checkBoxExNested.UseVisualStyleBackColor = true;
		this.checkBoxExNestOfSel.AutoSize = true;
		this.checkBoxExNestOfSel.Location = new System.Drawing.Point(6, 128);
		this.checkBoxExNestOfSel.Name = "checkBoxExNestOfSel";
		this.checkBoxExNestOfSel.Size = new System.Drawing.Size(110, 16);
		this.checkBoxExNestOfSel.TabIndex = 0;
		this.checkBoxExNestOfSel.Text = "Nest of Selected";
		this.checkBoxExNestOfSel.UseVisualStyleBackColor = true;
		this.checkBoxExHostOfSel.AutoSize = true;
		this.checkBoxExHostOfSel.Location = new System.Drawing.Point(6, 107);
		this.checkBoxExHostOfSel.Name = "checkBoxExHostOfSel";
		this.checkBoxExHostOfSel.Size = new System.Drawing.Size(110, 16);
		this.checkBoxExHostOfSel.TabIndex = 0;
		this.checkBoxExHostOfSel.Text = "Host of Selected";
		this.checkBoxExHostOfSel.UseVisualStyleBackColor = true;
		this.checkBoxExHost.AutoSize = true;
		this.checkBoxExHost.Location = new System.Drawing.Point(6, 192);
		this.checkBoxExHost.Name = "checkBoxExHost";
		this.checkBoxExHost.Size = new System.Drawing.Size(86, 16);
		this.checkBoxExHost.TabIndex = 0;
		this.checkBoxExHost.Text = "Host Family";
		this.checkBoxExHost.UseVisualStyleBackColor = true;
		this.checkBoxExWorkset.AutoSize = true;
		this.checkBoxExWorkset.Location = new System.Drawing.Point(6, 83);
		this.checkBoxExWorkset.Name = "checkBoxExWorkset";
		this.checkBoxExWorkset.Size = new System.Drawing.Size(97, 16);
		this.checkBoxExWorkset.TabIndex = 0;
		this.checkBoxExWorkset.Text = "Same Workset";
		this.checkBoxExWorkset.UseVisualStyleBackColor = true;
		this.checkBoxExFamily.AutoSize = true;
		this.checkBoxExFamily.Location = new System.Drawing.Point(6, 40);
		this.checkBoxExFamily.Name = "checkBoxExFamily";
		this.checkBoxExFamily.Size = new System.Drawing.Size(90, 16);
		this.checkBoxExFamily.TabIndex = 0;
		this.checkBoxExFamily.Text = "Same Family";
		this.checkBoxExFamily.UseVisualStyleBackColor = true;
		this.checkBoxExType.AutoSize = true;
		this.checkBoxExType.Location = new System.Drawing.Point(6, 62);
		this.checkBoxExType.Name = "checkBoxExType";
		this.checkBoxExType.Size = new System.Drawing.Size(81, 16);
		this.checkBoxExType.TabIndex = 0;
		this.checkBoxExType.Text = "Same Type";
		this.checkBoxExType.UseVisualStyleBackColor = true;
		this.checkBoxExCategory.AutoSize = true;
		this.checkBoxExCategory.Location = new System.Drawing.Point(6, 18);
		this.checkBoxExCategory.Name = "checkBoxExCategory";
		this.checkBoxExCategory.Size = new System.Drawing.Size(102, 16);
		this.checkBoxExCategory.TabIndex = 0;
		this.checkBoxExCategory.Text = "Same Category";
		this.checkBoxExCategory.UseVisualStyleBackColor = true;
		this.checkBoxHost.AutoSize = true;
		this.checkBoxHost.Location = new System.Drawing.Point(12, 84);
		this.checkBoxHost.Name = "checkBoxHost";
		this.checkBoxHost.Size = new System.Drawing.Size(86, 16);
		this.checkBoxHost.TabIndex = 0;
		this.checkBoxHost.Text = "Host Family";
		this.checkBoxHost.UseVisualStyleBackColor = true;
		this.checkBoxHost.CheckedChanged += new System.EventHandler(checkBoxHost_CheckedChanged);
		this.checkBoxNested.AutoSize = true;
		this.checkBoxNested.Location = new System.Drawing.Point(12, 62);
		this.checkBoxNested.Name = "checkBoxNested";
		this.checkBoxNested.Size = new System.Drawing.Size(98, 16);
		this.checkBoxNested.TabIndex = 0;
		this.checkBoxNested.Text = "Nested Family";
		this.checkBoxNested.UseVisualStyleBackColor = true;
		this.checkBoxNested.CheckedChanged += new System.EventHandler(checkBoxNested_CheckedChanged);
		this.buttonDeselect.BackColor = System.Drawing.Color.IndianRed;
		this.buttonDeselect.Location = new System.Drawing.Point(430, 615);
		this.buttonDeselect.Name = "buttonDeselect";
		this.buttonDeselect.Size = new System.Drawing.Size(75, 30);
		this.buttonDeselect.TabIndex = 6;
		this.buttonDeselect.Text = "Deselect";
		this.buttonDeselect.UseVisualStyleBackColor = false;
		this.buttonDeselect.Click += new System.EventHandler(buttonDeselect_Click);
		this.groupBoxExtended.Controls.Add(this.buttonExtend);
		this.groupBoxExtended.Controls.Add(this.groupBoxSelMethod);
		this.groupBoxExtended.Controls.Add(this.groupBoxSelectionRange);
		this.groupBoxExtended.Controls.Add(this.groupBoxCondition);
		this.groupBoxExtended.Location = new System.Drawing.Point(628, 165);
		this.groupBoxExtended.Name = "groupBoxExtended";
		this.groupBoxExtended.Size = new System.Drawing.Size(204, 411);
		this.groupBoxExtended.TabIndex = 7;
		this.groupBoxExtended.TabStop = false;
		this.groupBoxExtended.Text = "Extended Selection";
		this.buttonExtend.Location = new System.Drawing.Point(48, 374);
		this.buttonExtend.Name = "buttonExtend";
		this.buttonExtend.Size = new System.Drawing.Size(100, 30);
		this.buttonExtend.TabIndex = 7;
		this.buttonExtend.Text = "Extend Selection";
		this.buttonExtend.UseVisualStyleBackColor = true;
		this.buttonExtend.Click += new System.EventHandler(buttonExtend_Click);
		this.groupBoxSelMethod.Controls.Add(this.rBNewSel);
		this.groupBoxSelMethod.Controls.Add(this.rBExtendSel);
		this.groupBoxSelMethod.Location = new System.Drawing.Point(9, 306);
		this.groupBoxSelMethod.Name = "groupBoxSelMethod";
		this.groupBoxSelMethod.Size = new System.Drawing.Size(189, 62);
		this.groupBoxSelMethod.TabIndex = 6;
		this.groupBoxSelMethod.TabStop = false;
		this.groupBoxSelMethod.Text = "Selection Method";
		this.rBNewSel.AutoSize = true;
		this.rBNewSel.Location = new System.Drawing.Point(11, 40);
		this.rBNewSel.Name = "rBNewSel";
		this.rBNewSel.Size = new System.Drawing.Size(96, 16);
		this.rBNewSel.TabIndex = 0;
		this.rBNewSel.TabStop = true;
		this.rBNewSel.Text = "New Selection";
		this.rBNewSel.UseVisualStyleBackColor = true;
		this.rBExtendSel.AutoSize = true;
		this.rBExtendSel.Location = new System.Drawing.Point(11, 18);
		this.rBExtendSel.Name = "rBExtendSel";
		this.rBExtendSel.Size = new System.Drawing.Size(109, 16);
		this.rBExtendSel.TabIndex = 0;
		this.rBExtendSel.TabStop = true;
		this.rBExtendSel.Text = "Extend Selection";
		this.rBExtendSel.UseVisualStyleBackColor = true;
		this.groupBoxSelectionRange.Controls.Add(this.rBAllProjectE);
		this.groupBoxSelectionRange.Controls.Add(this.rBCurrentViewE);
		this.groupBoxSelectionRange.Location = new System.Drawing.Point(9, 18);
		this.groupBoxSelectionRange.Name = "groupBoxSelectionRange";
		this.groupBoxSelectionRange.Size = new System.Drawing.Size(189, 63);
		this.groupBoxSelectionRange.TabIndex = 5;
		this.groupBoxSelectionRange.TabStop = false;
		this.groupBoxSelectionRange.Text = "Selection Range";
		this.rBAllProjectE.AutoSize = true;
		this.rBAllProjectE.Location = new System.Drawing.Point(8, 40);
		this.rBAllProjectE.Name = "rBAllProjectE";
		this.rBAllProjectE.Size = new System.Drawing.Size(77, 16);
		this.rBAllProjectE.TabIndex = 0;
		this.rBAllProjectE.TabStop = true;
		this.rBAllProjectE.Text = "All Project";
		this.rBAllProjectE.UseVisualStyleBackColor = true;
		this.rBCurrentViewE.AutoSize = true;
		this.rBCurrentViewE.Location = new System.Drawing.Point(8, 18);
		this.rBCurrentViewE.Name = "rBCurrentViewE";
		this.rBCurrentViewE.Size = new System.Drawing.Size(90, 16);
		this.rBCurrentViewE.TabIndex = 0;
		this.rBCurrentViewE.TabStop = true;
		this.rBCurrentViewE.Text = "Current View";
		this.rBCurrentViewE.UseVisualStyleBackColor = true;
		this.groupBoxListingOption.Controls.Add(this.rBListMaterial);
		this.groupBoxListingOption.Controls.Add(this.rBListPhaseDemolished);
		this.groupBoxListingOption.Controls.Add(this.rBListPhaseCreated);
		this.groupBoxListingOption.Controls.Add(this.rBListChanged);
		this.groupBoxListingOption.Controls.Add(this.rBListOwner);
		this.groupBoxListingOption.Controls.Add(this.rBListLevel);
		this.groupBoxListingOption.Controls.Add(this.rBListType);
		this.groupBoxListingOption.Controls.Add(this.rBListCreated);
		this.groupBoxListingOption.Controls.Add(this.rBListWorkset);
		this.groupBoxListingOption.Location = new System.Drawing.Point(418, 108);
		this.groupBoxListingOption.Name = "groupBoxListingOption";
		this.groupBoxListingOption.Size = new System.Drawing.Size(194, 215);
		this.groupBoxListingOption.TabIndex = 9;
		this.groupBoxListingOption.TabStop = false;
		this.groupBoxListingOption.Text = "Grouping Option";
		this.rBListMaterial.AutoSize = true;
		this.rBListMaterial.Location = new System.Drawing.Point(12, 193);
		this.rBListMaterial.Name = "rBListMaterial";
		this.rBListMaterial.Size = new System.Drawing.Size(64, 16);
		this.rBListMaterial.TabIndex = 17;
		this.rBListMaterial.TabStop = true;
		this.rBListMaterial.Text = "Material";
		this.rBListMaterial.UseVisualStyleBackColor = true;
		this.rBListMaterial.CheckedChanged += new System.EventHandler(rBListMaterial_CheckedChanged);
		this.rBListPhaseDemolished.AutoSize = true;
		this.rBListPhaseDemolished.Location = new System.Drawing.Point(12, 172);
		this.rBListPhaseDemolished.Name = "rBListPhaseDemolished";
		this.rBListPhaseDemolished.Size = new System.Drawing.Size(117, 16);
		this.rBListPhaseDemolished.TabIndex = 16;
		this.rBListPhaseDemolished.TabStop = true;
		this.rBListPhaseDemolished.Text = "Demolished Phase";
		this.rBListPhaseDemolished.UseVisualStyleBackColor = true;
		this.rBListPhaseDemolished.CheckedChanged += new System.EventHandler(radioButtonPhase_CheckedChanged);
		this.rBListPhaseCreated.AutoSize = true;
		this.rBListPhaseCreated.Location = new System.Drawing.Point(12, 150);
		this.rBListPhaseCreated.Name = "rBListPhaseCreated";
		this.rBListPhaseCreated.Size = new System.Drawing.Size(98, 16);
		this.rBListPhaseCreated.TabIndex = 16;
		this.rBListPhaseCreated.TabStop = true;
		this.rBListPhaseCreated.Text = "Created Phase";
		this.rBListPhaseCreated.UseVisualStyleBackColor = true;
		this.rBListPhaseCreated.CheckedChanged += new System.EventHandler(radioButtonPhase_CheckedChanged);
		this.rBListChanged.AutoSize = true;
		this.rBListChanged.Location = new System.Drawing.Point(12, 128);
		this.rBListChanged.Name = "rBListChanged";
		this.rBListChanged.Size = new System.Drawing.Size(109, 16);
		this.rBListChanged.TabIndex = 14;
		this.rBListChanged.TabStop = true;
		this.rBListChanged.Text = "Last Changed by";
		this.rBListChanged.UseVisualStyleBackColor = true;
		this.rBListChanged.CheckedChanged += new System.EventHandler(rButtonListChanged_CheckedChanged);
		this.rBListOwner.AutoSize = true;
		this.rBListOwner.Location = new System.Drawing.Point(12, 106);
		this.rBListOwner.Name = "rBListOwner";
		this.rBListOwner.Size = new System.Drawing.Size(97, 16);
		this.rBListOwner.TabIndex = 13;
		this.rBListOwner.TabStop = true;
		this.rBListOwner.Text = "Current Owner";
		this.rBListOwner.UseVisualStyleBackColor = true;
		this.rBListOwner.CheckedChanged += new System.EventHandler(rButtonListOwner_CheckedChanged);
		this.rBListLevel.AutoSize = true;
		this.rBListLevel.Location = new System.Drawing.Point(12, 62);
		this.rBListLevel.Name = "rBListLevel";
		this.rBListLevel.Size = new System.Drawing.Size(50, 16);
		this.rBListLevel.TabIndex = 11;
		this.rBListLevel.TabStop = true;
		this.rBListLevel.Text = "Level";
		this.rBListLevel.UseVisualStyleBackColor = true;
		this.rBListLevel.CheckedChanged += new System.EventHandler(radioButtonListLevel_CheckedChanged);
		this.rBListType.AutoSize = true;
		this.rBListType.Location = new System.Drawing.Point(12, 18);
		this.rBListType.Name = "rBListType";
		this.rBListType.Size = new System.Drawing.Size(48, 16);
		this.rBListType.TabIndex = 10;
		this.rBListType.TabStop = true;
		this.rBListType.Text = "Type";
		this.rBListType.UseVisualStyleBackColor = true;
		this.rBListType.CheckedChanged += new System.EventHandler(radioButtonFamilyType_CheckedChanged);
		this.rBListCreated.AutoSize = true;
		this.rBListCreated.Location = new System.Drawing.Point(12, 84);
		this.rBListCreated.Name = "rBListCreated";
		this.rBListCreated.Size = new System.Drawing.Size(79, 16);
		this.rBListCreated.TabIndex = 10;
		this.rBListCreated.TabStop = true;
		this.rBListCreated.Text = "Created by";
		this.rBListCreated.UseVisualStyleBackColor = true;
		this.rBListCreated.CheckedChanged += new System.EventHandler(radioButtonUser_CheckedChanged);
		this.rBListWorkset.AutoSize = true;
		this.rBListWorkset.Location = new System.Drawing.Point(12, 40);
		this.rBListWorkset.Name = "rBListWorkset";
		this.rBListWorkset.Size = new System.Drawing.Size(64, 16);
		this.rBListWorkset.TabIndex = 10;
		this.rBListWorkset.TabStop = true;
		this.rBListWorkset.Text = "Workset";
		this.rBListWorkset.UseVisualStyleBackColor = true;
		this.rBListWorkset.CheckedChanged += new System.EventHandler(radioButtonWorkset_CheckedChanged);
		this.checkBoxInstance.AutoSize = true;
		this.checkBoxInstance.Location = new System.Drawing.Point(216, 19);
		this.checkBoxInstance.Name = "checkBoxInstance";
		this.checkBoxInstance.Size = new System.Drawing.Size(104, 16);
		this.checkBoxInstance.TabIndex = 18;
		this.checkBoxInstance.Text = "Show Instances";
		this.checkBoxInstance.UseVisualStyleBackColor = true;
		this.checkBoxInstance.CheckedChanged += new System.EventHandler(checkBoxInstance_CheckedChanged);
		this.groupBoxSearch.Controls.Add(this.rBSearchFamily);
		this.groupBoxSearch.Controls.Add(this.rBSearchType);
		this.groupBoxSearch.Controls.Add(this.comboBoxSearch);
		this.groupBoxSearch.Controls.Add(this.buttonReset);
		this.groupBoxSearch.Controls.Add(this.buttonSearch);
		this.groupBoxSearch.Controls.Add(this.textBoxSearch);
		this.groupBoxSearch.Location = new System.Drawing.Point(628, 12);
		this.groupBoxSearch.Name = "groupBoxSearch";
		this.groupBoxSearch.Size = new System.Drawing.Size(204, 137);
		this.groupBoxSearch.TabIndex = 12;
		this.groupBoxSearch.TabStop = false;
		this.groupBoxSearch.Text = "Search";
		this.rBSearchFamily.AutoSize = true;
		this.rBSearchFamily.Location = new System.Drawing.Point(98, 75);
		this.rBSearchFamily.Name = "rBSearchFamily";
		this.rBSearchFamily.Size = new System.Drawing.Size(90, 16);
		this.rBSearchFamily.TabIndex = 3;
		this.rBSearchFamily.TabStop = true;
		this.rBSearchFamily.Text = "Family Name";
		this.rBSearchFamily.UseVisualStyleBackColor = true;
		this.rBSearchType.AutoSize = true;
		this.rBSearchType.Location = new System.Drawing.Point(11, 76);
		this.rBSearchType.Name = "rBSearchType";
		this.rBSearchType.Size = new System.Drawing.Size(81, 16);
		this.rBSearchType.TabIndex = 3;
		this.rBSearchType.TabStop = true;
		this.rBSearchType.Text = "Type Name";
		this.rBSearchType.UseVisualStyleBackColor = true;
		this.comboBoxSearch.FormattingEnabled = true;
		this.comboBoxSearch.Location = new System.Drawing.Point(12, 43);
		this.comboBoxSearch.Name = "comboBoxSearch";
		this.comboBoxSearch.Size = new System.Drawing.Size(121, 20);
		this.comboBoxSearch.TabIndex = 2;
		this.buttonReset.Location = new System.Drawing.Point(106, 101);
		this.buttonReset.Name = "buttonReset";
		this.buttonReset.Size = new System.Drawing.Size(75, 25);
		this.buttonReset.TabIndex = 1;
		this.buttonReset.Text = "Reset";
		this.buttonReset.UseVisualStyleBackColor = true;
		this.buttonReset.Click += new System.EventHandler(buttonReset_Click);
		this.buttonSearch.Location = new System.Drawing.Point(5, 101);
		this.buttonSearch.Name = "buttonSearch";
		this.buttonSearch.Size = new System.Drawing.Size(75, 25);
		this.buttonSearch.TabIndex = 1;
		this.buttonSearch.Text = "Search";
		this.buttonSearch.UseVisualStyleBackColor = true;
		this.buttonSearch.Click += new System.EventHandler(buttonSearch_Click);
		this.textBoxSearch.Location = new System.Drawing.Point(12, 18);
		this.textBoxSearch.Name = "textBoxSearch";
		this.textBoxSearch.Size = new System.Drawing.Size(159, 19);
		this.textBoxSearch.TabIndex = 0;
		this.buttonExcel.Location = new System.Drawing.Point(537, 579);
		this.buttonExcel.Name = "buttonExcel";
		this.buttonExcel.Size = new System.Drawing.Size(75, 30);
		this.buttonExcel.TabIndex = 0;
		this.buttonExcel.Text = "Excel";
		this.buttonExcel.UseVisualStyleBackColor = true;
		this.buttonExcel.Click += new System.EventHandler(buttonExcel_Click);
		this.buttonClose.Location = new System.Drawing.Point(537, 615);
		this.buttonClose.Name = "buttonClose";
		this.buttonClose.Size = new System.Drawing.Size(75, 30);
		this.buttonClose.TabIndex = 14;
		this.buttonClose.Text = "Close";
		this.buttonClose.UseVisualStyleBackColor = true;
		this.buttonClose.Click += new System.EventHandler(buttonCancel_Click);
		this.groupBoxFiltering.Controls.Add(this.checkBoxNested);
		this.groupBoxFiltering.Controls.Add(this.checkBoxWarnings);
		this.groupBoxFiltering.Controls.Add(this.checkBoxInplace);
		this.groupBoxFiltering.Controls.Add(this.checkBoxHost);
		this.groupBoxFiltering.Location = new System.Drawing.Point(418, 338);
		this.groupBoxFiltering.Name = "groupBoxFiltering";
		this.groupBoxFiltering.Size = new System.Drawing.Size(194, 105);
		this.groupBoxFiltering.TabIndex = 15;
		this.groupBoxFiltering.TabStop = false;
		this.groupBoxFiltering.Text = "Filtering Option";
		this.checkBoxWarnings.AutoSize = true;
		this.checkBoxWarnings.Location = new System.Drawing.Point(12, 40);
		this.checkBoxWarnings.Name = "checkBoxWarnings";
		this.checkBoxWarnings.Size = new System.Drawing.Size(94, 16);
		this.checkBoxWarnings.TabIndex = 0;
		this.checkBoxWarnings.Text = "Has Warnings";
		this.checkBoxWarnings.UseVisualStyleBackColor = true;
		this.checkBoxWarnings.CheckedChanged += new System.EventHandler(checkBoxWarnings_CheckedChanged);
		this.checkBoxInplace.AutoSize = true;
		this.checkBoxInplace.Location = new System.Drawing.Point(12, 18);
		this.checkBoxInplace.Name = "checkBoxInplace";
		this.checkBoxInplace.Size = new System.Drawing.Size(94, 16);
		this.checkBoxInplace.TabIndex = 0;
		this.checkBoxInplace.Text = "In-Place Only";
		this.checkBoxInplace.UseVisualStyleBackColor = true;
		this.checkBoxInplace.CheckedChanged += new System.EventHandler(checkBoxInplace_CheckedChanged);
		this.buttonRefresh.Location = new System.Drawing.Point(337, 12);
		this.buttonRefresh.Name = "buttonRefresh";
		this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
		this.buttonRefresh.TabIndex = 16;
		this.buttonRefresh.Text = "Refresh";
		this.buttonRefresh.UseVisualStyleBackColor = true;
		this.buttonRefresh.Click += new System.EventHandler(buttonRefresh_Click);
		this.buttonAdvanced.Font = new System.Drawing.Font("MS UI Gothic", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
		this.buttonAdvanced.Location = new System.Drawing.Point(643, 599);
		this.buttonAdvanced.Name = "buttonAdvanced";
		this.buttonAdvanced.Size = new System.Drawing.Size(47, 23);
		this.buttonAdvanced.TabIndex = 33;
		this.buttonAdvanced.Text = ">>";
		this.buttonAdvanced.UseVisualStyleBackColor = true;
		this.buttonAdvanced.Click += new System.EventHandler(buttonAdvanced_Click);
		this.buttonCollapse.BackColor = System.Drawing.Color.Transparent;
		this.buttonCollapse.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.buttonCollapse.FlatAppearance.BorderSize = 0;
		this.buttonCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		//this.buttonCollapse.Image = ONES.Properties.Resources.collapse16;
		this.buttonCollapse.Location = new System.Drawing.Point(102, 11);
		this.buttonCollapse.Name = "buttonCollapse";
		this.buttonCollapse.Size = new System.Drawing.Size(23, 23);
		this.buttonCollapse.TabIndex = 0;
		this.buttonCollapse.UseVisualStyleBackColor = false;
		this.buttonCollapse.Click += new System.EventHandler(buttonCollapse_Click);
		this.buttonExpand.BackColor = System.Drawing.Color.Transparent;
		this.buttonExpand.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.buttonExpand.FlatAppearance.BorderSize = 0;
		this.buttonExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		//this.buttonExpand.Image = ONES.Properties.Resources.expand16;
		this.buttonExpand.Location = new System.Drawing.Point(75, 11);
		this.buttonExpand.Name = "buttonExpand";
		this.buttonExpand.Size = new System.Drawing.Size(23, 23);
		this.buttonExpand.TabIndex = 0;
		this.buttonExpand.UseVisualStyleBackColor = false;
		this.buttonExpand.Click += new System.EventHandler(buttonExpand_Click);
		this.buttonUncheck.BackColor = System.Drawing.Color.Transparent;
		this.buttonUncheck.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.buttonUncheck.FlatAppearance.BorderSize = 0;
		this.buttonUncheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		//this.buttonUncheck.Image = ONES.Properties.Resources.uncheckall16;
		this.buttonUncheck.Location = new System.Drawing.Point(41, 11);
		this.buttonUncheck.Name = "buttonUncheck";
		this.buttonUncheck.Size = new System.Drawing.Size(23, 23);
		this.buttonUncheck.TabIndex = 0;
		this.buttonUncheck.UseVisualStyleBackColor = false;
		this.buttonUncheck.Click += new System.EventHandler(buttonUncheck_Click);
		this.buttonCheck.BackColor = System.Drawing.Color.Transparent;
		this.buttonCheck.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.buttonCheck.FlatAppearance.BorderSize = 0;
		this.buttonCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		//this.buttonCheck.Image = ONES.Properties.Resources.checkall16;
		this.buttonCheck.Location = new System.Drawing.Point(12, 11);
		this.buttonCheck.Name = "buttonCheck";
		this.buttonCheck.Size = new System.Drawing.Size(23, 23);
		this.buttonCheck.TabIndex = 0;
		this.buttonCheck.UseVisualStyleBackColor = false;
		this.buttonCheck.Click += new System.EventHandler(buttonCheck_Click);
		this.buttonRemove.BackColor = System.Drawing.Color.Transparent;
		this.buttonRemove.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.buttonRemove.FlatAppearance.BorderSize = 0;
		this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		//this.buttonRemove.Image = ONES.Properties.Resources.remove16;
		this.buttonRemove.Location = new System.Drawing.Point(131, 11);
		this.buttonRemove.Name = "buttonRemove";
		this.buttonRemove.Size = new System.Drawing.Size(23, 23);
		this.buttonRemove.TabIndex = 0;
		this.buttonRemove.UseVisualStyleBackColor = false;
		this.buttonRemove.Click += new System.EventHandler(buttonRemove_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(844, 661);
		base.Controls.Add(this.checkBoxInstance);
		base.Controls.Add(this.buttonRefresh);
		base.Controls.Add(this.buttonAdvanced);
		base.Controls.Add(this.groupBoxFiltering);
		base.Controls.Add(this.buttonExcel);
		base.Controls.Add(this.buttonClose);
		base.Controls.Add(this.groupBoxSearch);
		base.Controls.Add(this.groupBoxListingOption);
		base.Controls.Add(this.groupBoxExtended);
		base.Controls.Add(this.buttonDeselect);
		base.Controls.Add(this.buttonRemove);
		base.Controls.Add(this.buttonCollapse);
		base.Controls.Add(this.buttonExpand);
		base.Controls.Add(this.buttonUncheck);
		base.Controls.Add(this.buttonCheck);
		base.Controls.Add(this.buttonSelect);
		base.Controls.Add(this.groupBoxSelection);
		base.Controls.Add(this.labelCount);
		base.Controls.Add(this.treeViewElements);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "SelectFilterForm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Select Filter";
		base.Load += new System.EventHandler(SelectFilterForm_Load);
		this.groupBoxSelection.ResumeLayout(false);
		this.groupBoxSelection.PerformLayout();
		this.groupBoxCondition.ResumeLayout(false);
		this.groupBoxCondition.PerformLayout();
		this.groupBoxExtended.ResumeLayout(false);
		this.groupBoxSelMethod.ResumeLayout(false);
		this.groupBoxSelMethod.PerformLayout();
		this.groupBoxSelectionRange.ResumeLayout(false);
		this.groupBoxSelectionRange.PerformLayout();
		this.groupBoxListingOption.ResumeLayout(false);
		this.groupBoxListingOption.PerformLayout();
		this.groupBoxSearch.ResumeLayout(false);
		this.groupBoxSearch.PerformLayout();
		this.groupBoxFiltering.ResumeLayout(false);
		this.groupBoxFiltering.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
