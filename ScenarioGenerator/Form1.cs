﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using GameGlobal;
using GameObjects;

namespace ScenarioGenerator
{
    public partial class Form1 : Form
    {
        private GameScenario scen = new GameScenario(null);
        private String openedFileName;
        private String commonDataFileName;
        private OleDbConnectionStringBuilder connectionStringBuilder;

        public Form1()
        {
            InitializeComponent();

            this.connectionStringBuilder = new OleDbConnectionStringBuilder();
            this.connectionStringBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
            this.commonDataFileName = Application.StartupPath + "/GameData/Common/CommonData.mdb";
            this.connectionStringBuilder.DataSource = this.commonDataFileName;
            new GlobalVariables().InitialGlobalVariables();
            this.scen.GameCommonData.LoadFromDatabase(this.connectionStringBuilder.ConnectionString, this.scen);
        }

        private void btnLoadScenario_Click(object sender, EventArgs e)
        {
            this.openScenFileDialog.InitialDirectory = ".";
            if (this.openScenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblFilename.Text = "正讀取劇本檔案......";

                this.connectionStringBuilder.DataSource = this.openScenFileDialog.FileName;
                this.openedFileName = this.openScenFileDialog.FileName;
                this.scen.LoadGameScenarioFromDatabase(this.connectionStringBuilder.ConnectionString);

                lblFilename.Text = this.scen.ScenarioTitle;
                btnGenerate.Enabled = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            lblFilename.Text = "正生成劇本...";

            generateScenario();

            lblFilename.Text = "正保存劇本...";

            String date = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
            this.scen.ScenarioTitle += "﹝生成於" + date + "﹞";

            String generatedFileName = this.openedFileName.Substring(0, this.openedFileName.Length - 4) + "-" + date + ".mdb";

            File.Copy(Application.StartupPath + "/GameData/Common/SaveTemplate.mdb", generatedFileName);

            this.connectionStringBuilder.DataSource = generatedFileName;
            this.scen.SaveGameScenarioToDatabase(this.connectionStringBuilder.ConnectionString, true, this.scen.UsingOwnCommonData);

            MessageBox.Show("劇本已生成，檔案為" + generatedFileName);

            lblFilename.Text = this.openedFileName;
        }

        private void generateScenario()
        {
        }

    }
}
