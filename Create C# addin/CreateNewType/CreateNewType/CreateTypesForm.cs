using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml;

namespace CreateNewType;

public class CreateTypesForm : System.Windows.Forms.Form
{
	
	private Command m_Command = null;

	private FamilySymbol symbol;

	private UIDocument m_uidocument;

	private IContainer components = null;

	private System.Windows.Forms.ComboBox comboBox_FamilyMaps;

	private Label label2;

	private Button button_OK;

	private Button button_Cancel;

	private Label label_SheetName;

	private System.Windows.Forms.ComboBox comboBox_SheetName;

	public CreateTypesForm(Command command)
	{
		m_Command = command;
		m_uidocument = m_Command.ActiveUIDocument;
		InitializeComponent();
		List<Family> dataSource = command.FamilyMaps.OrderBy((Family e) => ((Element)e).Name).ToList();
		comboBox_FamilyMaps.DataSource = dataSource;
		comboBox_FamilyMaps.DisplayMember = "Name";
		comboBox_SheetName.DataSource = command.WorkSheets;
	}

	private void comboBox_FamilyMaps_SelectedValueChanged(object sender, EventArgs e)
	{
        System.Windows.Forms.ComboBox comboBox = sender as System.Windows.Forms.ComboBox;
		if (comboBox.SelectedItem == null)
		{
			return;
		}
		object selectedItem = comboBox.SelectedItem;
		Family val = (Family)((selectedItem is Family) ? selectedItem : null);
		ISet<ElementId> familySymbolIds = val.GetFamilySymbolIds();
		using IEnumerator<ElementId> enumerator = familySymbolIds.GetEnumerator();
		if (enumerator.MoveNext())
		{
			ElementId current = enumerator.Current;
			ref FamilySymbol reference = ref symbol;
			Element element = m_Command.ActiveUIDocument.Document.GetElement(current);
			reference = (FamilySymbol)(object)((element is FamilySymbol) ? element : null);
		}
	}

	private void button_OK_Click(object sender, EventArgs e)
	{
		m_Command.m_WorkSheetName = (comboBox_SheetName.SelectedValue as ExcelWorksheet).Name;
		ref Family family = ref m_Command.m_Family;
		object selectedItem = comboBox_FamilyMaps.SelectedItem;
		family = (Family)((selectedItem is Family) ? selectedItem : null);
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
		this.comboBox_FamilyMaps = new System.Windows.Forms.ComboBox();
		this.label2 = new System.Windows.Forms.Label();
		this.button_OK = new System.Windows.Forms.Button();
		this.button_Cancel = new System.Windows.Forms.Button();
		this.label_SheetName = new System.Windows.Forms.Label();
		this.comboBox_SheetName = new System.Windows.Forms.ComboBox();
		base.SuspendLayout();
		this.comboBox_FamilyMaps.FormattingEnabled = true;
		this.comboBox_FamilyMaps.Location = new System.Drawing.Point(90, 17);
		this.comboBox_FamilyMaps.Name = "comboBox_FamilyMaps";
		this.comboBox_FamilyMaps.Size = new System.Drawing.Size(212, 21);
		this.comboBox_FamilyMaps.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(15, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(42, 13);
		this.label2.TabIndex = 4;
		this.label2.Text = "Family :";
		this.button_OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.button_OK.Location = new System.Drawing.Point(146, 74);
		this.button_OK.Name = "button_OK";
		this.button_OK.Size = new System.Drawing.Size(75, 23);
		this.button_OK.TabIndex = 5;
		this.button_OK.Text = "OK";
		this.button_OK.UseVisualStyleBackColor = true;
		this.button_OK.Click += new System.EventHandler(button_OK_Click);
		this.button_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.button_Cancel.Location = new System.Drawing.Point(227, 74);
		this.button_Cancel.Name = "button_Cancel";
		this.button_Cancel.Size = new System.Drawing.Size(75, 23);
		this.button_Cancel.TabIndex = 6;
		this.button_Cancel.Text = "Cancel";
		this.button_Cancel.UseVisualStyleBackColor = true;
		this.label_SheetName.AutoSize = true;
		this.label_SheetName.Location = new System.Drawing.Point(15, 47);
		this.label_SheetName.Name = "label_SheetName";
		this.label_SheetName.Size = new System.Drawing.Size(69, 13);
		this.label_SheetName.TabIndex = 14;
		this.label_SheetName.Text = "Sheet Name:";
		this.comboBox_SheetName.FormattingEnabled = true;
		this.comboBox_SheetName.Location = new System.Drawing.Point(90, 44);
		this.comboBox_SheetName.Name = "comboBox_SheetName";
		this.comboBox_SheetName.Size = new System.Drawing.Size(212, 21);
		this.comboBox_SheetName.TabIndex = 13;
		base.AcceptButton = this.button_OK;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.CancelButton = this.button_Cancel;
		base.ClientSize = new System.Drawing.Size(314, 109);
		base.Controls.Add(this.label_SheetName);
		base.Controls.Add(this.comboBox_SheetName);
		base.Controls.Add(this.button_Cancel);
		base.Controls.Add(this.button_OK);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.comboBox_FamilyMaps);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "CreateTypesForm";
		base.ShowIcon = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Create Type";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
