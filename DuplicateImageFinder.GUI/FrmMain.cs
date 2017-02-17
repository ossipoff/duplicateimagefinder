using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
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
		private List<string> imagePaths;
		private Int32 fileCounter;
		private Int32 filesToDelete;
		private string errorMsg = null;
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

			SetEnvironment(EnvironmentMode.New);
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

		private void ReportProgress(Int32 count, Int32 total)
		{
			var value = ((double)count / (double)total) * 100;
			var percentage = Convert.ToInt32(Math.Round(value, 0));

			if (this.statusStrip.InvokeRequired)
			{
				this.statusStrip.Invoke(new Action<Int32, Int32>(ReportProgress), new object[] { count, total });
			}
			else
			{
				this.progressBar.Value = percentage;
			}
		}

		private void ResetProgress()
		{
			fileCounter = 0;

			if (this.statusStrip.InvokeRequired)
			{
				this.statusStrip.Invoke((MethodInvoker)delegate() { ResetProgress(); });
			}
			else
			{
				this.progressBar.Value = 0;
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
				this.statusStrip.Invoke((MethodInvoker)delegate() { StartMarquee(); });
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

		private void AddToImageList(string path, Bitmap img)
		{
			if (listView.InvokeRequired)
			{
				listView.Invoke(new Action<string, Bitmap>(AddToImageList), new object[] { path, img });
			}
			else
			{
				imageList.Images.Add(path, img);
			}
		}

		private void AddToListView(ListViewItem lvItem)
		{
			if (listView.InvokeRequired)
			{
				listView.Invoke(new Action<ListViewItem>(AddToListView), new object[] { lvItem });
			}
			else
			{
				listView.BeginUpdate();
				listView.Items.Add(lvItem);
				listView.EndUpdate();
			}
		}

		private void DeleteFileAndItem()
		{
			if (listView.InvokeRequired)
			{
				listView.Invoke((MethodInvoker)delegate() { DeleteFileAndItem(); });
			}
			else
			{
				foreach (ListViewItem item in listView.Items)
				{
					if (item.Checked == true)
					{
						File.Delete(item.Name);
						listView.Items.RemoveByKey(item.Name);
						ReportProgress(++fileCounter, filesToDelete);
					}
				}
			}
		}

		private void SetEnvironment(EnvironmentMode mode)
		{
			switch (mode)
			{
				case EnvironmentMode.New:
					isDuplicatesFound = false;
					fileCounter = 0;
					filesToDelete = 0;

					if (imagePaths == null)
						imagePaths = new List<string>();
					else
						imagePaths.Clear();

					if (processedImagesList == null)
						processedImagesList = new Dictionary<string, List<string>>();
					else
						processedImagesList.Clear();

					listView.Items.Clear();
					imageList.Images.Clear();
					SetLocationText(Properties.Settings.Default.LastLocationText);

					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, true);

					SetStatusText("Ready");
					break;
				case EnvironmentMode.Scanning:
					StartMarquee();
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, false);
					SetStatusText(String.Format("Scanning {0} for images...", txtLocation.Text));
					break;
				case EnvironmentMode.ScanningCompleted:
					StopMarquee();
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, false);
					SetStatusText(String.Format("Scanning completed, total files found {0}", imagePaths.Count));
					break;
				case EnvironmentMode.ScanningErrorOrNoImage:
					StopMarquee();
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, true);
					SetStatusText(errorMsg);
					break;
				case EnvironmentMode.FindingDuplicates:
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, false);
					break;
				case EnvironmentMode.FindingDuplicatesCompleted:
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, false);
					break;
				case EnvironmentMode.GeneratingThumbnails:
					ResetProgress();
					SetStatusText("Generating thumbnails...");
					break;
				case EnvironmentMode.GeneratingThumbnailsCompleted:
					if (isDuplicatesFound)
					{
						EnableDisableGroup(grpBoxActions, true);
					}
					EnableDisableGroup(grpBoxScan, true);
					SetStatusText("Thumbnail generation completed");
					break;
				case EnvironmentMode.GeneratingThumbnailsErrorOrNoDuplicate:
					SetStatusText("No duplicate file found");
					ResetProgress();
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, true);
					break;
				case EnvironmentMode.RemovingDuplicateFiles:
					ResetProgress();
					SetStatusText("Removing selected files...");
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, false);
					break;
				case EnvironmentMode.RemovingDuplicateFilesCompleted:
					SetStatusText("All selected files removed");
					EnableDisableGroup(grpBoxActions, true);
					EnableDisableGroup(grpBoxScan, true);
					break;
				case EnvironmentMode.RemovingDuplicateFilesError:
					SetStatusText(errorMsg);
					EnableDisableGroup(grpBoxActions, false);
					EnableDisableGroup(grpBoxScan, true);
					break;
			}
		}
		#endregion

		#region Worker Event Handlers
		private void objDupImageFinder_ImageProcessed(object sender, Ossisoft.FileProcessedEventArgs e)
		{
			SetStatusText(String.Format("Finding duplicates, processed: {0}", e.FilePath));
			ReportProgress(++fileCounter, imagePaths.Count);
		}

		private void removeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (String.IsNullOrEmpty(errorMsg))
			{
				SetEnvironment(EnvironmentMode.RemovingDuplicateFilesCompleted);
			}
		}

		private void removeWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			SetEnvironment(EnvironmentMode.RemovingDuplicateFiles);

			try
			{
				DeleteFileAndItem();
				errorMsg = "";
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				SetEnvironment(EnvironmentMode.RemovingDuplicateFilesError);
			}
		}

		private void previewWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (isDuplicatesFound)
				SetEnvironment(EnvironmentMode.GeneratingThumbnailsCompleted);
		}

		private void previewWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				var duplicates = from grp in processedImagesList
								 where grp.Value.Count > 1
								 select grp;

				if (duplicates.Count() > 0)
				{
					SetEnvironment(EnvironmentMode.GeneratingThumbnails);
					isDuplicatesFound = true;

					// Calculating total files to generate thumbnails for
					int totalFiles = 0;
					foreach (var item in duplicates)
					{
						if (item.Value.Count > 1)
						{
							foreach (var path in item.Value)
							{
								++totalFiles;
							}
						}
					}

					foreach (var item in duplicates)
					{
						string oldFilePath = item.Value[0];

						foreach (var path in item.Value)
						{
							if (new FileInfo(oldFilePath).CreationTime > new FileInfo(path).CreationTime)
							{
								oldFilePath = path;
							}
						}

						foreach (var path in item.Value)
						{
							using (Bitmap img = (Bitmap)Image.FromFile(path))
							{
								AddToImageList(path, img);
							}

							ListViewItem lvItem = new ListViewItem();
							lvItem.Text = Path.GetFileName(path);
							lvItem.Name = path;
							lvItem.ImageKey = path;
							lvItem.ToolTipText = path;

							if (path != oldFilePath)
							{
								lvItem.Checked = true;
								++filesToDelete;
							}

							AddToListView(lvItem);
							ReportProgress(++fileCounter, totalFiles);
						}
					}
				}
				else
				{
					isDuplicatesFound = false;
					SetEnvironment(EnvironmentMode.GeneratingThumbnailsErrorOrNoDuplicate);
				}
			}
			catch (Exception ex)
			{
				isDuplicatesFound = false;
				errorMsg = "Error: Unknown";
				SetEnvironment(EnvironmentMode.GeneratingThumbnailsErrorOrNoDuplicate);
			}
		}

		private void duplicateFinderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (String.IsNullOrEmpty(errorMsg))
			{
				SetEnvironment(EnvironmentMode.FindingDuplicatesCompleted);
				previewWorker.RunWorkerAsync();
			}
		}

		private void duplicateFinderWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				processedImagesList = objDupImageFinder.FindDuplicates(imagePaths);
				errorMsg = string.Empty;
			}
			catch (Exception ex)
			{
				errorMsg = "Error: Unknown";
				SetEnvironment(EnvironmentMode.FindingDuplicatesError);
			}
		}

		private void scanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (string.IsNullOrEmpty(errorMsg))
			{
				if (imagePaths.Count > 0)
				{
					SetEnvironment(EnvironmentMode.ScanningCompleted);
					duplicateFinderWorker.RunWorkerAsync();
				}
				else
				{
					errorMsg = "No image file found";
					SetEnvironment(EnvironmentMode.ScanningErrorOrNoImage);
				}
			}
		}

		private void scanWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string directoryPath = Path.Combine(e.Argument.ToString());
			SetEnvironment(EnvironmentMode.Scanning);

			try
			{
				imagePaths.AddRange(Directory.GetFiles(directoryPath, "*.JPG", SearchOption.AllDirectories));
				imagePaths.AddRange(Directory.GetFiles(directoryPath, "*.JPEG", SearchOption.AllDirectories));
				imagePaths.AddRange(Directory.GetFiles(directoryPath, "*.PNG", SearchOption.AllDirectories));

				errorMsg = string.Empty;
			}
			catch (UnauthorizedAccessException ex)
			{
				errorMsg = String.Format("Error: {0}", ex.Message);
				SetEnvironment(EnvironmentMode.ScanningErrorOrNoImage);
			}
			catch
			{
				errorMsg = String.Format("Error: Unknown");
				SetEnvironment(EnvironmentMode.ScanningErrorOrNoImage);
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
			SetEnvironment(EnvironmentMode.New);

			if (Directory.Exists(Path.Combine(txtLocation.Text)))
			{
				scanWorker.RunWorkerAsync(Path.Combine(txtLocation.Text));
			}
			else
			{
				SetStatusText("Error: Invalid images location");
			}
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			if (listView.Items.Count > 0)
			{
				foreach (ListViewItem item in listView.Items)
				{
					item.Checked = true;
				}
			}
		}

		private void btnClearAll_Click(object sender, EventArgs e)
		{
			if (listView.Items.Count > 0)
			{
				foreach (ListViewItem item in listView.Items)
				{
					item.Checked = false;
				}
			}
		}

		private void btnDeleteSelected_Click(object sender, EventArgs e)
		{
			DialogResult dr = MessageBox.Show("Are you ABSOLUTELY sure?\nThis action can't be undone.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (dr == DialogResult.OK)
			{
				removeWorker.RunWorkerAsync();
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
		GeneratingThumbnails,
		GeneratingThumbnailsCompleted,
		GeneratingThumbnailsErrorOrNoDuplicate,
		RemovingDuplicateFiles,
		RemovingDuplicateFilesCompleted,
		RemovingDuplicateFilesError,
		FindingDuplicatesError
	}
}