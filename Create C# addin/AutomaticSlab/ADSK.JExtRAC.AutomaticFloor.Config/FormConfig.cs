using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ADSK.JExtCom.Dnf;
using ADSK.JExtRAC.AutomaticFloor.Components;
using ADSK.JExtRAC.AutomaticFloor.Entities;
using ADSK.JExtRAC.AutomaticFloor.Utils;

namespace ADSK.JExtRAC.AutomaticFloor.Config;

public class FormConfig : Form
{
	private ADSK.JExtRAC.AutomaticFloor.Components.Attribute _CmpAttribute;

	private DtSlabType _EntDtSlabType;

	private DtCmd _EntDtCmd;

	private IContainer components;

	private Button btnCancel;

	private Button btnOK;

	private Label lblSlabType;

	private Label lblHeightOffset;

	private Label lblHeightOffsetUnit;

	private ComboBox cboSlabType;

	private TextBox txtHeightOffset;

	private Label lblLock;

	private CheckBox chbLock;

	private ComboBox cboDirectionAngle;

	private Label lblDirectionAngle;

	private Label lblDegree;

	private ErrorProvider errPvd;

	public FormConfig(ADSK.JExtRAC.AutomaticFloor.Components.Attribute cmpAttribute, DtSlabType entDtSlabType, DtCmd entDtCmd, eFloorType eFloorType)
	{
		InitializeComponent();
		_CmpAttribute = cmpAttribute;
		_EntDtSlabType = entDtSlabType;
		_EntDtCmd = entDtCmd;
		SetText(eFloorType);
		SetData();
	}

	private void SetText(eFloorType eFloorType)
	{
		switch (eFloorType)
		{
		case eFloorType.Arch:
			Text = _CmpAttribute.ResourceText("IDS_TXT_ARCHITECTURE") + $"[Ver.{Assembly.GetExecutingAssembly().GetName().Version}]";
			lblSlabType.Text = _CmpAttribute.ResourceText("IDS_TXT_ARCHITECTURE_FTYPE");
			lblLock.Text = _CmpAttribute.ResourceText("IDS_TXT_LOCK_ARCHITECT");
			lblDirectionAngle.Text = _CmpAttribute.ResourceText("IDS_TXT_SLAB_DIRECTION");
			break;
		case eFloorType.Struct:
			Text = _CmpAttribute.ResourceText("IDS_TXT_STRUCTURAL") + $"[Ver.{Assembly.GetExecutingAssembly().GetName().Version}]";
			lblSlabType.Text = _CmpAttribute.ResourceText("IDS_TXT_STRUCTURAL_FTYPE");
			lblLock.Text = _CmpAttribute.ResourceText("IDS_TXT_LOCK_STRUCTURAL");
			lblDirectionAngle.Text = _CmpAttribute.ResourceText("IDS_TXT_SLAB_DIRECTION");
			break;
		default:
			Text = _CmpAttribute.ResourceText("IDS_TXT_FOUDATION_SLAB") + $"[Ver.{Assembly.GetExecutingAssembly().GetName().Version}]";
			lblSlabType.Text = _CmpAttribute.ResourceText("IDS_TXT_FOUDATION_SLAB_FTYPE");
			lblLock.Text = _CmpAttribute.ResourceText("IDS_TXT_LOCK_FOUNDATION_SLAB");
			lblDirectionAngle.Text = _CmpAttribute.ResourceText("IDS_TXT_SLAB_DIRECTION");
			break;
		}
		lblHeightOffset.Text = _CmpAttribute.ResourceText("IDS_TXT_LEVELHEIGHTOFFSET");
		lblHeightOffsetUnit.Text = _CmpAttribute.ResourceText("IDS_UNIT_MM");
		lblDegree.Text = _CmpAttribute.ResourceText("IDS_DEGREE");
		btnOK.Text = _CmpAttribute.ResourceText("IDS_TXT_OK");
		btnCancel.Text = _CmpAttribute.ResourceText("IDS_TXT_CANCEL");
		base.Icon = _CmpAttribute.ResourceImage("IDI_SUBS_ICON") as Icon;
	}

	private void SetData()
	{
		double num = 0.0;
		string text = "";
		cboSlabType.DataSource = _EntDtSlabType.Data;
		cboSlabType.DisplayMember = _EntDtSlabType.ColNameName;
		cboSlabType.ValueMember = _EntDtSlabType.ColNameID;
		cboDirectionAngle.DataSource = _EntDtCmd.DataDirection;
		cboDirectionAngle.DisplayMember = "Name";
		cboDirectionAngle.ValueMember = "Value";
		if (_EntDtCmd.Data.Count >= 4)
		{
			text = _EntDtCmd.Data[0];
			num = 0.0;
			if (UtilValue.IsNumber(text))
			{
				num = double.Parse(text);
			}
			txtHeightOffset.Text = num.ToString();
			text = _EntDtCmd.Data[1];
			if (text == null)
			{
				text = "";
			}
			cboSlabType.Text = text;
			if (_EntDtCmd.Data[2] == "true")
			{
				chbLock.Checked = true;
			}
			else
			{
				chbLock.Checked = false;
			}
			if (!string.IsNullOrEmpty(_EntDtCmd.Data[3]))
			{
				cboDirectionAngle.SelectedValue = _EntDtCmd.Data[3].ToString();
				cboDirectionAngle.Text = _EntDtCmd.Data[3].ToString();
			}
			else if (cboDirectionAngle.Items.Count > 0)
			{
				cboDirectionAngle.SelectedValue = "0";
			}
		}
	}

	private void GetData()
	{
		double num = 0.0;
		string text = "";
		int num2 = 0;
		if (_EntDtCmd.Data.Count >= 4)
		{
			num = 0.0;
			text = txtHeightOffset.Text;
			if (UtilValue.IsNumber(text))
			{
				num = double.Parse(text);
			}
			_EntDtCmd.Data[0] = num.ToString();
			text = cboSlabType.Text;
			if (text == null)
			{
				text = "";
			}
			_EntDtCmd.Data[1] = text;
			_EntDtCmd.Data[2] = (chbLock.Checked ? "true" : "false");
			_EntDtCmd.DegreeAngle = 0.0;
			if (cboDirectionAngle.SelectedValue != null)
			{
				_EntDtCmd.DegreeAngle = double.Parse(cboDirectionAngle.SelectedValue.ToString());
			}
			else
			{
				_EntDtCmd.DegreeAngle = double.Parse(cboDirectionAngle.Text.ToString());
			}
			_EntDtCmd.Data[3] = _EntDtCmd.DegreeAngle.ToString();
		}
		num2 = 0;
		text = cboSlabType.SelectedValue.ToString();
		if (UtilValue.IsInteger(text))
		{
			num2 = int.Parse(text);
		}
		_EntDtSlabType.GetWorkElem(num2);
	}

	private void ChkErrPvd()
	{
		errPvd.SetError(cboDirectionAngle, _EntDtCmd.SetErrPvdDecimalText(cboDirectionAngle.Text.Trim()));
	}

	private bool GetErrPvd()
	{
		bool result = false;
		if (!string.IsNullOrEmpty(errPvd.GetError(cboDirectionAngle)))
		{
			return result;
		}
		return true;
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
		ChkErrPvd();
		if (GetErrPvd())
		{
			GetData();
			base.DialogResult = DialogResult.OK;
			Close();
		}
	}

	private void txtHeightOffset_KeyPress(object sender, KeyPressEventArgs e)
	{
		Common.NumberCheck(sender, e, allowNegativeValue: true);
	}

	private void cboDirectionAngle_Validated(object sender, EventArgs e)
	{
		errPvd.SetError(cboDirectionAngle, _EntDtCmd.SetErrPvdDecimalText(cboDirectionAngle.Text.Trim()));
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
		this.components = new System.ComponentModel.Container();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnOK = new System.Windows.Forms.Button();
		this.lblSlabType = new System.Windows.Forms.Label();
		this.lblHeightOffset = new System.Windows.Forms.Label();
		this.lblHeightOffsetUnit = new System.Windows.Forms.Label();
		this.cboSlabType = new System.Windows.Forms.ComboBox();
		this.txtHeightOffset = new System.Windows.Forms.TextBox();
		this.lblLock = new System.Windows.Forms.Label();
		this.chbLock = new System.Windows.Forms.CheckBox();
		this.cboDirectionAngle = new System.Windows.Forms.ComboBox();
		this.lblDirectionAngle = new System.Windows.Forms.Label();
		this.lblDegree = new System.Windows.Forms.Label();
		this.errPvd = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errPvd).BeginInit();
		base.SuspendLayout();
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Location = new System.Drawing.Point(336, 130);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 25);
		this.btnCancel.TabIndex = 6;
		this.btnCancel.Text = "btnCancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnOK.Location = new System.Drawing.Point(255, 130);
		this.btnOK.Name = "btnOK";
		this.btnOK.Size = new System.Drawing.Size(75, 25);
		this.btnOK.TabIndex = 5;
		this.btnOK.Text = "btnOK";
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		this.lblSlabType.AutoSize = true;
		this.lblSlabType.Location = new System.Drawing.Point(12, 16);
		this.lblSlabType.Name = "lblSlabType";
		this.lblSlabType.Size = new System.Drawing.Size(62, 13);
		this.lblSlabType.TabIndex = 0;
		this.lblSlabType.Text = "lblSlabType";
		this.lblHeightOffset.AutoSize = true;
		this.lblHeightOffset.Location = new System.Drawing.Point(12, 44);
		this.lblHeightOffset.Name = "lblHeightOffset";
		this.lblHeightOffset.Size = new System.Drawing.Size(76, 13);
		this.lblHeightOffset.TabIndex = 2;
		this.lblHeightOffset.Text = "lblHeightOffset";
		this.lblHeightOffsetUnit.AutoSize = true;
		this.lblHeightOffsetUnit.Location = new System.Drawing.Point(213, 44);
		this.lblHeightOffsetUnit.Name = "lblHeightOffsetUnit";
		this.lblHeightOffsetUnit.Size = new System.Drawing.Size(95, 13);
		this.lblHeightOffsetUnit.TabIndex = 4;
		this.lblHeightOffsetUnit.Text = "lblHeightOffsetUnit";
		this.cboSlabType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboSlabType.FormattingEnabled = true;
		this.cboSlabType.Location = new System.Drawing.Point(111, 13);
		this.cboSlabType.Name = "cboSlabType";
		this.cboSlabType.Size = new System.Drawing.Size(300, 21);
		this.cboSlabType.TabIndex = 1;
		this.txtHeightOffset.Location = new System.Drawing.Point(111, 41);
		this.txtHeightOffset.Name = "txtHeightOffset";
		this.txtHeightOffset.Size = new System.Drawing.Size(100, 20);
		this.txtHeightOffset.TabIndex = 3;
		this.txtHeightOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtHeightOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtHeightOffset_KeyPress);
		this.lblLock.AutoSize = true;
		this.lblLock.Location = new System.Drawing.Point(12, 74);
		this.lblLock.Name = "lblLock";
		this.lblLock.Size = new System.Drawing.Size(41, 13);
		this.lblLock.TabIndex = 17;
		this.lblLock.Text = "lblLock";
		this.chbLock.AutoSize = true;
		this.chbLock.Location = new System.Drawing.Point(111, 74);
		this.chbLock.Name = "chbLock";
		this.chbLock.Size = new System.Drawing.Size(15, 14);
		this.chbLock.TabIndex = 16;
		this.chbLock.UseVisualStyleBackColor = true;
		this.cboDirectionAngle.FormattingEnabled = true;
		this.cboDirectionAngle.Location = new System.Drawing.Point(111, 94);
		this.cboDirectionAngle.Name = "cboDirectionAngle";
		this.cboDirectionAngle.Size = new System.Drawing.Size(100, 21);
		this.cboDirectionAngle.TabIndex = 19;
		this.cboDirectionAngle.Validated += new System.EventHandler(cboDirectionAngle_Validated);
		this.lblDirectionAngle.AutoSize = true;
		this.lblDirectionAngle.Location = new System.Drawing.Point(12, 97);
		this.lblDirectionAngle.Name = "lblDirectionAngle";
		this.lblDirectionAngle.Size = new System.Drawing.Size(86, 13);
		this.lblDirectionAngle.TabIndex = 18;
		this.lblDirectionAngle.Text = "lblDirectionAngle";
		this.lblDegree.AutoSize = true;
		this.lblDegree.Location = new System.Drawing.Point(213, 94);
		this.lblDegree.Name = "lblDegree";
		this.lblDegree.Size = new System.Drawing.Size(52, 13);
		this.lblDegree.TabIndex = 20;
		this.lblDegree.Text = "lblDegree";
		this.errPvd.ContainerControl = this;
		base.AcceptButton = this.btnOK;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.btnCancel;
		base.ClientSize = new System.Drawing.Size(432, 167);
		base.Controls.Add(this.lblDegree);
		base.Controls.Add(this.cboDirectionAngle);
		base.Controls.Add(this.lblDirectionAngle);
		base.Controls.Add(this.lblLock);
		base.Controls.Add(this.chbLock);
		base.Controls.Add(this.txtHeightOffset);
		base.Controls.Add(this.cboSlabType);
		base.Controls.Add(this.lblHeightOffsetUnit);
		base.Controls.Add(this.lblHeightOffset);
		base.Controls.Add(this.lblSlabType);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOK);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "FormConfig";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "FormConfig";
		((System.ComponentModel.ISupportInitialize)this.errPvd).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
