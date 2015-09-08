using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            Rules.Add(new Rule { Name = "Home_Image", Width = 260, Height = 195, FileNameRule = "{SourceFileName}_home{SourceFileExtension}" });
            Rules.Add(new Rule { Name = "List_Image", Width = 325, Height = 195, FileNameRule = "{SourceFileName}_list{SourceFileExtension}" });
            Rules.Add(new Rule { Name = "Calculator1_Image", Width = 470, Height = 315, FileNameRule = "{SourceFileName}_calculator1{SourceFileExtension}" });
            Rules.Add(new Rule { Name = "calculator2_Image", Width = 95, Height = 62, FileNameRule = "{SourceFileName}_calculator2{SourceFileExtension}" });

            dgvRules.DataSource = Rules;
        }

        private void FillFiles()
        {
            if (Directory.Exists(txtFolderPath.Text.Trim()))
            {
                var lst = Directory.GetFiles(txtFolderPath.Text, "*", SearchOption.AllDirectories);

                //匹配以'\'开头的文件，文件名只能为guid并且是.jpg/.png/.gif/.jpen格式的文件
                Files = lst
                    .Where(p => Regex.IsMatch(p, @"\\[a-f0-9]{8}(-[a-f0-9]{4}){3}-[a-f0-9]{12}\.(jpg|png|gif|bmp|jpen)", RegexOptions.IgnoreCase))
                    .ToList();
            }
            else
            {
                Files = new List<string>();
            }
        }


        #endregion

        private void btnRun_Click(object sender, EventArgs e)
        {
            Console.ResetColor();

            FillFiles();

            if (Files.Count <= 0)
            {
                MessageBox.Show("没有获取到图片");
                return;
            }

            int fileCount = 0;
            int successGenFile = 0;            
            int failGenFile = 0;
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("A total of {0} picture files need to be processed.", Files.Count);
            var sw = new Stopwatch();
            sw.Restart();
            foreach (var oldFilePath in Files)
            {
                fileCount += 1;

                Console.WriteLine("\n{0}/{1} {2}", fileCount, Files.Count, oldFilePath);
                foreach (var rule in Rules)
                {
                    var fileInfo = new FileInfo(oldFilePath);

                    var newFilePath = Path.Combine(fileInfo.DirectoryName,
                        rule.FileNameRule
                        .Replace("{SourceFileName}", fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length, fileInfo.Extension.Length))
                        .Replace("{SourceFileExtension}", fileInfo.Extension)
                        );

                    var imageInfo = rule.Name + " w:" + rule.Width + " h:" + rule.Height + " Quality:" + rule.Quality;
                    try
                    {
                        Helper.Run(oldFilePath, newFilePath, rule.Width, rule.Height, rule.Quality);
                        successGenFile += 1;
                        Console.WriteLine("Generation Image\t{0}\t{1}", imageInfo, "Success!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generation Image\t{0}\t{1}", imageInfo, "Failure!");
                        failGenFile += 1;
                        sb.AppendFormat("\n\n Image Path:{0}, Type:{1}, Time:{2}, Message:{3}", oldFilePath, imageInfo, DateTime.Now, ex.ToString());
                        File.WriteAllText("GenImage_Log.txt", sb.ToString());
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("\n All completed！With a total of：{0}", sw.Elapsed);

            MessageBox.Show(string.Format("完成!\n 成功生成{0}个图片,生成失败图片{1}个! ",successGenFile, failGenFile));
        }
    }
}
