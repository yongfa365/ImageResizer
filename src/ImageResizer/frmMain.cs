using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageResizer
{
    public partial class frmMain : Form
    {
        private List<string> Files { get; set; }
        private List<Rule> Rules { get; set; }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            FillDefaultSettings();
        }
        #region Default Settings

        private void FillDefaultSettings()
        {
            txtFolderPath.Text = @"D:\123";

            Rules = new List<Rule>();
            Rules.Add(new Rule { Width = 260, Height = 159, FileNameRule = "{SourceFileName}_home{SourceFileExtension}" });
            //Rules.Add(new Rule { Width = 123, Height = 234, FileNameRule = "{SourceFileName}_home{SourceFileExtension}" });
            //Rules.Add(new Rule { Width = 123, Height = 234, FileNameRule = "{SourceFileName}_home{SourceFileExtension}" });
            //Rules.Add(new Rule { Width = 123, Height = 234, FileNameRule = "{SourceFileName}_home{SourceFileExtension}" });
            var lst = Directory.GetFiles(@"F:\Users\Desktop\magicdemo", "*", SearchOption.AllDirectories);
            Files = lst.ToList();


            dgvRules.DataSource = Rules;
        }

        #endregion

        private void btnRun_Click(object sender, EventArgs e)
        {
            foreach (var oldFilePath in Files)
            {
                foreach (var rule in Rules)
                {
                    var fileInfo = new FileInfo(oldFilePath);
                    var newFilePath = rule.FileNameRule
                        .Replace("{SourceFileName}", fileInfo.Name)
                        .Replace("{SourceFileExtension}", fileInfo.Extension);

                    Helper.Run(oldFilePath, newFilePath, rule.Width, rule.Height);
                }
            }

        }
    }
}
