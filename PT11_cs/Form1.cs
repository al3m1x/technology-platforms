using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace PT11_v3__WF_
{
    public partial class Form1 : Form
    {
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private async Task<long> CalculateBinomialCoefficientAsync(int n, int k)
        {
            if (k > n) throw new ArgumentException("K nie może być większe niż N.");
            if (k == 0 || k == n) return 1;

            var numeratorTask = Task.Run(() => CalculateLicznik(n, k));
            var denominatorTask = Task.Run(() => CalculateMianownik(k));

            long numerator = await numeratorTask;
            long denominator = await denominatorTask;

            return numerator / denominator;
        }

        private long CalculateBinomialCoefficientDelegaty(int n, int k)
        {
            if (k > n) throw new ArgumentException("K nie może być większe niż N.");
            if (k == 0 || k == n) return 1;

            Func<int, int, long> numeratorDelegate = CalculateLicznik;
            Func<int, long> denominatorDelegate = CalculateMianownik;

            IAsyncResult numeratorResult = numeratorDelegate.BeginInvoke(n, k, null, null);
            IAsyncResult denominatorResult = denominatorDelegate.BeginInvoke(k, null, null);

            long numerator = numeratorDelegate.EndInvoke(numeratorResult);
            long denominator = denominatorDelegate.EndInvoke(denominatorResult);

            return numerator / denominator;
        }

        private async Task<long> CalculateBinomialCoefficientTask(int n, int k)
        {
            if (k > n) throw new ArgumentException("K nie może być większe niż N.");
            if (k == 0 || k == n) return 1;

            var numeratorTask = Task.Run(() => CalculateLicznik(n, k));
            var denominatorTask = Task.Run(() => CalculateMianownik(k));

            long numerator = await numeratorTask;
            long denominator = await denominatorTask;

            return numerator / denominator;
        }

        private long CalculateLicznik(int n, int k)
        {
            long result = 1;
            for (int i = 0; i < k; i++)
            {
                result *= (n - i);
            }
            return result;
        }

        private long CalculateMianownik(int k)
        {
            long result = 1;
            for (int i = 1; i <= k; i++)
            {
                result *= i;
            }
            return result;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int n = (int)e.Argument;
            long result = CalculateFibonacci(n, backgroundWorker);
            e.Result = result;
        }

        private long CalculateFibonacci(int n, BackgroundWorker worker)
        {
            if (n <= 0) return 0;
            if (n == 1) return 1;

            long a = 0;
            long b = 1;
            for (int i = 2; i <= n; i++)
            {
                long temp = a + b;
                a = b;
                b = temp;
                Thread.Sleep(5);
                worker.ReportProgress((i * 100) / n);
            }
            return b;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            richTextBox1.AppendText($"\r\nWynik: {e.Result}\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                int n;
                if (int.TryParse(textBox1.Text, out n))
                {
                    backgroundWorker.RunWorkerAsync(n);
                    richTextBox1.AppendText($"Obliczanie Fibonacci dla n={n}...\r\n");
                }
                else
                {
                    richTextBox1.AppendText($"Nieprawidłowa wartość n: {textBox1.Text}\r\n");
                }
            }
            else
            {
                richTextBox1.AppendText("Obliczenia są już w toku...\r\n");
            }
        }

        private async void Button2_Click(object sender, EventArgs e)
        {
            int N = 10;
            int K = 3;

            try
            {
                long resultTask = await CalculateBinomialCoefficientTask(N, K);
                richTextBox1.AppendText($"Implementacja za pomocą Task:\r\n");
                richTextBox1.AppendText($"Symbol Newtona dla N={N} i K={K} to: {resultTask}\r\n\r\n");

                long resultDelegate = CalculateBinomialCoefficientDelegaty(N, K);
                richTextBox1.AppendText($"Implementacja za pomocą delegat:\r\n");
                richTextBox1.AppendText($"Symbol Newtona dla N={N} i K={K} to: {resultDelegate}\r\n\r\n");

                long resultAwait = await CalculateBinomialCoefficientAsync(N, K);
                richTextBox1.AppendText($"Implementacja za pomocą async-await:\r\n");
                richTextBox1.AppendText($"Symbol Newtona dla N={N} i K={K} obliczany asynchronicznie to: {resultAwait}\r\n\r\n");
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText($"Błąd podczas obliczania: {ex.Message}\r\n");
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string selectedFolder = SelectFolder();
            if (string.IsNullOrEmpty(selectedFolder))
            {
                richTextBox1.AppendText("Nie wybrano katalogu.");
                return;
            }

            CompressFiles(selectedFolder);

            richTextBox1.AppendText("Operacja kompresji zakończona.");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            string selectedFolder = SelectFolder();
            if (string.IsNullOrEmpty(selectedFolder))
            {
                richTextBox1.AppendText("Nie wybrano katalogu.");
                return;
            }

            DecompressFiles(selectedFolder);

            richTextBox1.AppendText("Operacja dekompresji zakończona.");
        }

        private string SelectFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    return dialog.SelectedPath;
                }
                return null;
            }
        }

        private void CompressFiles(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            Parallel.ForEach(files, file =>
            {
                CompressFile(file);
            });
        }

        private void CompressFile(string filePath)
        {
            if (!filePath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
            {
                string gzipFilePath = $"{filePath}.gz";

                using (var inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var outputFileStream = new FileStream(gzipFilePath, FileMode.Create, FileAccess.Write))
                    {
                        using (var gzipStream = new GZipStream(outputFileStream, CompressionMode.Compress))
                        {
                            inputFileStream.CopyTo(gzipStream);
                        }
                    }
                }
            }
        }

        private void DecompressFiles(string folderPath)
        {
            string[] gzipFiles = Directory.GetFiles(folderPath, "*.gz");

            Parallel.ForEach(gzipFiles, gzipFile =>
            {
                DecompressFile(gzipFile);
            });
        }

        private void DecompressFile(string gzipFilePath)
        {
            string originalFilePath = gzipFilePath.Substring(0, gzipFilePath.Length - 3);

            using (var inputFileStream = new FileStream(gzipFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var outputFileStream = new FileStream(originalFilePath, FileMode.Create, FileAccess.Write))
                {
                    using (var gzipStream = new GZipStream(inputFileStream, CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(outputFileStream);
                    }
                }
            }
        }

    }
}
