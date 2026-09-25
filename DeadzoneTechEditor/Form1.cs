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
                selectedSavePath = fileDialog.FileName;
                saveByteData = reader.Read(selectedSavePath);

                if (ChecksumService.IsChecksumValid(saveByteData))
                {
                    lblStatus.Text = TechService.GetTech(saveByteData).ToString();
                }
                else
                {
                    lblStatus.Text = "Invalid or unsupported save file";
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

            if (!int.TryParse(txtNewTech.Text, out int newTech) || newTech < 0)
            {
                lblStatus.Text = "Enter a valid Tech amount";
                return;
            }
            if (!ChecksumService.IsChecksumValid(saveByteData))
            {
                lblStatus.Text = "Invalid save checksum";
                return;
            }

            File.Copy(selectedSavePath, selectedSavePath + ".bak", true);

            TechService.SetTech(saveByteData, newTech);
            ChecksumService.WriteChecksum(saveByteData);

            File.WriteAllBytes(selectedSavePath, saveByteData);

            byte[] writtenSave = File.ReadAllBytes(selectedSavePath);

            if(!ChecksumService.IsChecksumValid(writtenSave))
            {
                lblStatus.Text = "Patch verification failed";
                return;
            }

            lblStatus.Text = $"Success - Tech set to {newTech:N0}";
        }
    }
}
