using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            txtFolderPath.Text = @"F:\Users\Desktop\magicdemo";

            Rules = new List<Rule>();
            Rules.Add(new Rule { Width = 260, Height = 195, FileNameRule = "{SourceFileName}_home{SourceFileExtension}" });
            Rules.Add(new Rule { Width = 260, Height = 195, FileNameRule = "{SourceFileName}_list{SourceFileExtension}" });
            Rules.Add(new Rule { Width = 470, Height = 315, FileNameRule = "{SourceFileName}_calculator1{SourceFileExtension}" });
            Rules.Add(new Rule { Width = 95, Height = 62, FileNameRule = "{SourceFileName}_calculator2{SourceFileExtension}" });

            var lst = Directory.GetFiles(txtFolderPath.Text, "*", SearchOption.AllDirectories);



            dgvRules.DataSource = Rules;
        }

        private void FillFiles()
        {
            var lst = Directory.GetFiles(txtFolderPath.Text, "*", SearchOption.AllDirectories);
            Files = lst
                .Where(p => Regex.IsMatch(p, @".+\\.{36}\.(jpg|png|gif|bmp|jpeg)", RegexOptions.IgnoreCase))
                .ToList();
        }


        #endregion

        private void btnRun_Click(object sender, EventArgs e)
        {
            FillFiles();

            foreach (var oldFilePath in Files)
            {
                foreach (var rule in Rules)
                {
                    var fileInfo = new FileInfo(oldFilePath);

                    var newFilePath = Path.Combine(fileInfo.DirectoryName,
                        rule.FileNameRule
                        .Replace("{SourceFileName}", fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length, fileInfo.Extension.Length))
                        .Replace("{SourceFileExtension}", fileInfo.Extension)
                        );

                    Helper.Run(oldFilePath, newFilePath, rule.Width, rule.Height, rule.Quality);
                }
            }
            MessageBox.Show("OK");
        }
    }
}
