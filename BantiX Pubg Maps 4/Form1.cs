using System;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BX_PUBG_MAP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Text = "PUBG Map Viewer";
            notifyIcon1.Visible = true;

            notifyIcon1.BalloonTipTitle = "Добро пожаловать!";
            notifyIcon1.BalloonTipText = "Программа BantiX Pubg Maps успешно запущена.";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(3000);
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }
        private void Erangel_Click(object sender, EventArgs e) { }
        private void Rondo_Click(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            Erangel.Visible = true;
            Paramo.Visible = false;
            Deston.Visible = false;
            DestonLabel.Visible = false;
            Rondo.Visible = false;
            Vikendi.Visible = false;
            Taego.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Paramo.Visible = true;
            Erangel.Visible = false;
            Deston.Visible = false;
            DestonLabel.Visible = false;
            Rondo.Visible = false;
            Vikendi.Visible = false;
            Taego.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Deston.Visible = true;
            Paramo.Visible = false;
            Erangel.Visible = false;
            DestonLabel.Visible = true;
            Rondo.Visible = false;
            Vikendi.Visible = false;
            Taego.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Rondo.Visible = true;
            Deston.Visible = false;
            Paramo.Visible = false;
            Erangel.Visible = false;
            DestonLabel.Visible = false;
            Vikendi.Visible = false;
            Taego.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Vikendi.Visible = true;
            Rondo.Visible = false;
            Deston.Visible = false;
            Paramo.Visible = false;
            Erangel.Visible = false;
            DestonLabel.Visible = false;
            Taego.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Taego.Visible = true;
            Vikendi.Visible = false;
            Rondo.Visible = false;
            Deston.Visible = false;
            Paramo.Visible = false;
            Erangel.Visible = false;
            DestonLabel.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Taego.Visible = false;
            Vikendi.Visible = false;
            Rondo.Visible = false;
            Deston.Visible = false;
            Paramo.Visible = false;
            Erangel.Visible = false;
            DestonLabel.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async Task CheckForUpdateAsync(string owner, string repo)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("BX-PUBG-MAP-Updater");

                string url = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
                string json = await client.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                string tag = doc.RootElement.GetProperty("tag_name").GetString();
                string latestVersion = tag.TrimStart('v');
                string currentVersion = Application.ProductVersion;

                if (latestVersion != currentVersion)
                {
                    if (doc.RootElement.TryGetProperty("assets", out JsonElement assets) && assets.GetArrayLength() > 0)
                    {
                        string downloadUrl = assets[0].GetProperty("browser_download_url").GetString();
                        string newExePath = Path.Combine(Application.StartupPath, "BX-PUBG-MAP_new.exe");

                        using var wc = new WebClient();
                        wc.DownloadFile(downloadUrl, newExePath);

                        notifyIcon1.BalloonTipTitle = "Обновление";
                        notifyIcon1.BalloonTipText = $"Новая версия {latestVersion} загружена.";
                        notifyIcon1.ShowBalloonTip(3000);

                        string currentExePath = Application.ExecutablePath;
                        string backupPath = Path.Combine(Application.StartupPath, "BX-PUBG-MAP_old.exe");

                        try
                        {
                            File.Move(currentExePath, backupPath);
                            File.Move(newExePath, currentExePath);

                            System.Diagnostics.Process.Start(currentExePath);
                            Application.Exit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при замене файла: {ex.Message}", "Обновление");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Файл обновления не найден в релизе.", "Обновление");
                    }
                }
                else
                {
                    MessageBox.Show("У вас уже последняя версия.", "Обновление");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке обновлений:\n{ex.Message}", "Обновление");
            }
        }

        private async void CheckUpdates_Click(object sender, EventArgs e)
        {
            await CheckForUpdateAsync("Pahalin", "BantiX-Pubg-Maps");
        }
    }
}
