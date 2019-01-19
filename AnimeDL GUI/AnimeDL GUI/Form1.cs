﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json;


namespace AnimeDL_GUI
{
    public partial class Form1 : Form
    {
        public const string configurationFile = "configuration.json";
        ConfigurationData conf;

        private ConfigurationData LoadJson(string jsonFile)
        {
            try
            {
                using (StreamReader r = new StreamReader(jsonFile))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<ConfigurationData>(json);
                }
            }
            catch
            {
                return new ConfigurationData();
            }
        }

        void LoadQuality(string quality)
        {
            switch (quality)
            {
                case "480p": { RB480p.Select(); break; }
                case "720p": { RB720p.Select(); break; }
                case "1080p": { RB1080p.Select(); break; }
                default: break;
            }
        }

        public Form1()
        {
            InitializeComponent();
            conf = LoadJson(configurationFile);

            animeList.Items.AddRange(conf.animeList.ToArray());
            LoadQuality(conf.quality);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string adjustedText = animeInput.Text.ToString();
            // some characters don't work well with python script
            adjustedText = adjustedText.Replace('–', '-');
            adjustedText = adjustedText.Replace('’', '\'');
            animeList.Items.Add(adjustedText);
            conf.animeList.Add(adjustedText);
            animeInput.Text = "";
        }

        private void animeInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void animeList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            conf.animeList.RemoveAt(animeList.SelectedIndex);
            animeList.Items.RemoveAt(animeList.SelectedIndex);
        }

        private void ExportConfigToJson(ConfigurationData a_config)
        {
            string json = JsonConvert.SerializeObject(a_config);
            System.IO.File.WriteAllText(configurationFile, json);
        }

        private void SaveList_Click(object sender, EventArgs e)
        {
            ExportConfigToJson(conf);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            conf.quality = RB480p.Text;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            conf.quality = RB720p.Text;
        }


        public string run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "PATH_TO_PYTHON_EXE";
            start.Arguments = string.Format("\"{0}\" \"{1}\"", cmd, args);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    return result;
                }
            }
        }

        private void download_Click(object sender, EventArgs e)
        {
            //run_cmd();
        }

        private void RB1080p_CheckedChanged(object sender, EventArgs e)
        {
            conf.quality = RB1080p.Text;
        }
    }
}
