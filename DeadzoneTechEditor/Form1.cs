namespace DeadzoneTechEditor
{
    public partial class Form1 : Form
    {
        private string? selectedSavePath;
        private byte[]? saveByteData;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectSave_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            SaveFileReader reader = new SaveFileReader();

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] candidate = reader.Read(fileDialog.FileName);
                    if (!ChecksumService.IsChecksumValid(candidate))
                        throw new InvalidDataException("Invalid save checksum");
                    long tech = TechService.GetTech(candidate);
                    selectedSavePath = fileDialog.FileName;
                    saveByteData = candidate;
                    lblStatus.Text = tech.ToString("N0");
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
                {
                    selectedSavePath = null;
                    saveByteData = null;
                    lblStatus.Text = ex.Message;
                }

            }
            else
            {
                lblStatus.Text = "No file selected";
            }
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            if (saveByteData is null || selectedSavePath is null)
            {
                lblStatus.Text = "Select a save first";
                return;
            }

            if (!long.TryParse(txtNewTech.Text, out long newTech) || newTech < 0)
            {
                lblStatus.Text = "Enter a valid Tech amount";
                return;
            }

            string? temporaryPath = null;
            try
            {
                // Reread immediately before editing: the game may have saved since selection.
                byte[] original = File.ReadAllBytes(selectedSavePath);
                if (!ChecksumService.IsChecksumValid(original))
                    throw new InvalidDataException("Invalid save checksum");
                TechService.GetTech(original); // Reject unsupported records.
                byte[] patched = (byte[])original.Clone();
                TechService.SetTech(patched, newTech);
                ChecksumService.WriteChecksum(patched);
                if (!ChecksumService.IsChecksumValid(patched) || TechService.GetTech(patched) != newTech)
                    throw new InvalidDataException("Patch verification failed");

                // Preserve any existing backup and retain this exact pre-edit save.
                string backupPath = selectedSavePath + ".bak";
                if (File.Exists(backupPath))
                    backupPath += "." + Guid.NewGuid().ToString("N");
                string backupTemp = backupPath + ".tmp";
                try
                {
                    File.WriteAllBytes(backupTemp, original);
                    File.Move(backupTemp, backupPath);
                }
                finally
                {
                    if (File.Exists(backupTemp)) File.Delete(backupTemp);
                }
                temporaryPath = selectedSavePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
                File.WriteAllBytes(temporaryPath, patched);
                File.Move(temporaryPath, selectedSavePath, true);
                temporaryPath = null;
                byte[] written = File.ReadAllBytes(selectedSavePath);
                if (!ChecksumService.IsChecksumValid(written) || TechService.GetTech(written) != newTech)
                    throw new InvalidDataException("Patch verification failed; restore the .bak backup.");
                saveByteData = written;
                lblStatus.Text = $"Success - Tech set to {newTech:N0}. Backup: {Path.GetFileName(backupPath)}";
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException or OverflowException)
            {
                lblStatus.Text = ex.Message;
            }
            finally
            {
                if (temporaryPath is not null && File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }
    }
}
