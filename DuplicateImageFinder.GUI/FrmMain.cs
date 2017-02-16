using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DuplicateImageFinder.GUI
{
	public partial class FrmMain : Form
	{
		#region Private Members
		private BackgroundWorker scanWorker;
		private BackgroundWorker duplicateFinderWorker;
		private BackgroundWorker previewWorker;
		private BackgroundWorker removeWorker;
		private Ossisoft.DuplicateImageFinder objDupImageFinder;

		private bool isDuplicatesFound;
		private ImageList imageList;
		private List<string> imagePaths;
		private Int32 fileCounter;
		private Dictionary<string, List<string>> processedImagesList;
		#endregion

		public FrmMain()
		{
			InitializeComponent();
			Init();
		}

		#region Private Methods
		private void Init()
		{
			scanWorker = new BackgroundWorker();
			scanWorker.DoWork += scanWorker_DoWork;
			scanWorker.RunWorkerCompleted += scanWorker_RunWorkerCompleted;

			duplicateFinderWorker = new BackgroundWorker();
			duplicateFinderWorker.DoWork += duplicateFinderWorker_DoWork;
			duplicateFinderWorker.RunWorkerCompleted += duplicateFinderWorker_RunWorkerCompleted;

			previewWorker = new BackgroundWorker();
			previewWorker.DoWork += previewWorker_DoWork;
			previewWorker.RunWorkerCompleted += previewWorker_RunWorkerCompleted;

			removeWorker = new BackgroundWorker();
			removeWorker.DoWork += removeWorker_DoWork;
			removeWorker.RunWorkerCompleted += removeWorker_RunWorkerCompleted;

			objDupImageFinder = new Ossisoft.DuplicateImageFinder();
			objDupImageFinder.ImageProcessed += objDupImageFinder_ImageProcessed;

			SetEnvironment(EnvironmentMode.New, "Ready");
		}

		private void SetStatusText(string text)
		{
			if (this.statusStrip.InvokeRequired)
			{
				this.statusStrip.Invoke(new Action<string>(SetStatusText), new object[] { text });
			}
			else
			{
				this.lblStatus.Text = text;
			}
		}

		private void SetLocationText(string text)
		{
			if (this.txtLocation.InvokeRequired)
			{
				this.statusStrip.Invoke(new Action<string>(SetLocationText), new object[] { text });
			}
			else
			{
				this.txtLocation.Text = text;
				Properties.Settings.Default.LastLocationText = text;
				Properties.Settings.Default.Save();
			}
		}

		private void ReportProgress(Int32 p)
		{
			var value = ((double)p / (double)imagePaths.Count) * 100;
			var percentage = Convert.ToInt32(Math.Round(value, 0));

			if (this.statusStrip.InvokeRequired)
			{
				this.statusStrip.Invoke(new Action<Int32>(ReportProgress), new object[] { p });
			}
			else
			{
				this.progressBar.Value = percentage;
			}
		}

		private void EnableDisableGroup(GroupBox grpBox, bool enabled)
		{
			if (grpBox.InvokeRequired)
			{
				grpBox.Invoke(new Action<GroupBox, bool>(EnableDisableGroup), new object[] { grpBox, enabled });
			}
			else
			{
				grpBox.Enabled = enabled;
			}
		}

		private void StartMarquee()
		{
			if (this.statusStrip.InvokeRequired)
			{
				this.statusStrip.Invoke((MethodInvoker)StartMarquee);
			}
			else
			{
				this.progressBar.Style = ProgressBarStyle.Marquee;
			}
		}

		private void StopMarquee()
		{
			if (this.statusStrip.InvokeRequired)
			{
				this.statusStrip.Invoke((MethodInvoker)StopMarquee);
			}
			else
			{
				this.progressBar.Style = ProgressBarStyle.Blocks;
			}
		}

		private void SetEnvironment(EnvironmentMode mode, string statusText)
		{
			switch (mode)
			{
				case EnvironmentMode.New:
					isDuplicatesFound = false;
					fileCounter = 0;

					imageList = new ImageList();
					imageList.ColorDepth = ColorDepth.Depth32Bit;
					imageList.ImageSize = new Size(150, 150);

					imagePaths = new List<string>();
					processedImagesList = new Dictionary<string, List<string>>();

					SetLocationText(Properties.Settings.Default.LastLocationText);
					SetStatusText("Ready");
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, true);
					break;
				case EnvironmentMode.Scanning:
					SetStatusText(statusText);
					StartMarquee();
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, false);
					break;
				case EnvironmentMode.ScanningCompleted:
					SetStatusText(statusText);
					StopMarquee();
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, false);
					break;
				case EnvironmentMode.ScanningErrorOrNoImage:
					SetStatusText(statusText);
					StopMarquee();
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, true);
					break;
				case EnvironmentMode.FindingDuplicates:
					SetStatusText(statusText);
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, false);
					break;
				case EnvironmentMode.FindingDuplicatesCompleted:
					SetStatusText(statusText);
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, true);
					break;
				case EnvironmentMode.GeneratingThumbnails:
					SetStatusText(statusText);
					break;
			}
		}
		#endregion

		#region Worker Event Handlers
		private void objDupImageFinder_ImageProcessed(object sender, Ossisoft.FileProcessedEventArgs e)
		{
			SetStatusText(String.Format("Finding duplicates, processed: {0}", e.FilePath));
			ReportProgress(++fileCounter);
		}

		private void removeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

		}

		private void removeWorker_DoWork(object sender, DoWorkEventArgs e)
		{

		}

		private void previewWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

		}

		private void previewWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			var duplicates = from grp in processedImagesList
							 where grp.Value.Count > 1
							 select grp;

			/*
			 *  TODO:
			if (duplicates.Count() > 0)
			{
				SetEnvironment(EnvironmentMode.GeneratingThumbnails, "Generating thumbnails...");
				foreach (var item in duplicates)
				{
					if (item.Value.Count > 1)
					{
						Console.WriteLine(string.Format("{0} ({1})", item.Key, item.Value.Count));
						foreach (var path in item.Value)
						{
							Console.WriteLine(string.Format("\t{0}", path));
						}
					}
				}
			}
			else
			{
				Console.WriteLine("No duplicates found");
			}
			 */
		}

		private void duplicateFinderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			SetEnvironment(EnvironmentMode.FindingDuplicatesCompleted, "All images processed for duplicates");
		}

		private void duplicateFinderWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			SetEnvironment(EnvironmentMode.FindingDuplicates, "Started processing images for duplicates");
			processedImagesList = objDupImageFinder.FindDuplicates(imagePaths);
		}

		private void scanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (imagePaths.Count > 0)
			{
				SetEnvironment(EnvironmentMode.ScanningCompleted, String.Format("Scanning completed"));
				duplicateFinderWorker.RunWorkerAsync();
			}
			else
			{
				SetEnvironment(EnvironmentMode.ScanningErrorOrNoImage, String.Format("No images found at this location"));
			}
		}

		private void scanWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string directoryPath = Path.Combine(e.Argument.ToString());
			SetEnvironment(EnvironmentMode.Scanning, String.Format("Scanning for images in {0}", directoryPath));

			try
			{
				imagePaths.AddRange(Directory.GetFiles(directoryPath, "*.JPG", SearchOption.AllDirectories));
				imagePaths.AddRange(Directory.GetFiles(directoryPath, "*.JPEG", SearchOption.AllDirectories));
				imagePaths.AddRange(Directory.GetFiles(directoryPath, "*.PNG", SearchOption.AllDirectories));
			}
			catch (UnauthorizedAccessException ex)
			{
				SetEnvironment(EnvironmentMode.ScanningErrorOrNoImage, String.Format("Error: {0}", ex.Message));
			}
			catch
			{
				SetEnvironment(EnvironmentMode.ScanningErrorOrNoImage, String.Format("Error: {0}", "Unknown"));
			}
		}
		#endregion

		#region Buttons Click Event Handlers
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.ShowNewFolderButton = false;
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				{
					SetLocationText(folderBrowserDialog.SelectedPath);
				}
			}
		}

		private void btnScan_Click(object sender, EventArgs e)
		{
			SetEnvironment(EnvironmentMode.New, "");

			if (Directory.Exists(Path.Combine(txtLocation.Text)))
			{
				scanWorker.RunWorkerAsync(Path.Combine(txtLocation.Text));
			}
			else
			{
				SetStatusText("Error: Invalid images location");
			}
		}
		#endregion
	}

	internal enum EnvironmentMode
	{
		New,
		Scanning,
		ScanningCompleted,
		ScanningErrorOrNoImage,
		FindingDuplicates,
		FindingDuplicatesCompleted,
		GeneratingThumbnails
	}
}
