using System;
using System.Globalization;
using System.Windows.Forms;
using Nikse.SubtitleEdit.Logic;

namespace Nikse.SubtitleEdit.Forms.DCinema
{
    public sealed partial class DCinemaPropertiesSmpte : DCinemaPropertiesForm
    {
        public DCinemaPropertiesSmpte()
        {
            InitializeComponent();

            var l = Configuration.Settings.Language.DCinemaProperties;
            Text = l.TitleSmpte;
            labelSubtitleID.Text = l.SubtitleId;
            labelMovieTitle.Text = l.MovieTitle;
            labelReelNumber.Text = l.ReelNumber;
            labelLanguage.Text = l.Language;
            labelIssueDate.Text = l.IssueDate;
            labelEditRate.Text = l.EditRate;
            if (!string.IsNullOrEmpty(l.TimeCodeRate)) //TODO: Remove in SE 3.4
                labelTimeCodeRate.Text = l.TimeCodeRate;
            if (!string.IsNullOrEmpty(l.StartTime)) //TODO: Remove in SE 3.4
                labelStartTime.Text = l.StartTime;
            groupBoxFont.Text = l.Font;
            labelFontId.Text = l.FontId;
            labelFontUri.Text = l.FontUri;
            labelFontColor.Text = l.FontColor;
            buttonFontColor.Text = l.ChooseColor;
            labelEffect.Text = l.FontEffect;
            labelEffectColor.Text = l.FontEffectColor;
            buttonFontEffectColor.Text = l.ChooseColor;
            labelFontSize.Text = l.FontSize;
            buttonGenerateID.Text = l.Generate;
            buttonGenFontUri.Text = l.Generate;

            foreach (CultureInfo x in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                comboBoxLanguage.Items.Add(x.TwoLetterISOLanguageName);
            }
            comboBoxLanguage.Sorted = true;

            var ss = Configuration.Settings.SubtitleSettings;
            if (!string.IsNullOrEmpty(ss.CurrentDCinemaSubtitleId))
            {
                textBoxSubtitleID.Text = ss.CurrentDCinemaSubtitleId;
                int number;
                if (int.TryParse(ss.CurrentDCinemaReelNumber, out number) &&
                    numericUpDownReelNumber.Minimum <= number && numericUpDownReelNumber.Maximum >= number)
                {
                    numericUpDownReelNumber.Value = number;
                }
                textBoxMovieTitle.Text = ss.CurrentDCinemaMovieTitle;
                comboBoxLanguage.Text = ss.CurrentDCinemaLanguage;
                textBoxFontID.Text = ss.CurrentDCinemaFontId;
                textBoxEditRate.Text = ss.CurrentDCinemaEditRate;
                comboBoxTimeCodeRate.Text = ss.CurrentDCinemaTimeCodeRate;

                timeUpDownStartTime.ForceHHMMSSFF();
                if (string.IsNullOrEmpty(ss.CurrentDCinemaStartTime))
                    ss.CurrentDCinemaStartTime = "00:00:00:00";
                timeUpDownStartTime.MaskedTextBox.Text = ss.CurrentDCinemaStartTime;

                textBoxFontUri.Text = ss.CurrentDCinemaFontUri;
                textBoxIssueDate.Text = ss.CurrentDCinemaIssueDate;
                panelFontColor.BackColor = ss.CurrentDCinemaFontColor;
                if (ss.CurrentDCinemaFontEffect == "border")
                    comboBoxFontEffect.SelectedIndex = 1;
                else if (ss.CurrentDCinemaFontEffect == "shadow")
                    comboBoxFontEffect.SelectedIndex = 2;
                else
                    comboBoxFontEffect.SelectedIndex = 0;
                panelFontEffectColor.BackColor = ss.CurrentDCinemaFontEffectColor;
                numericUpDownFontSize.Value = ss.CurrentDCinemaFontSize;
                if (numericUpDownTopBottomMargin.Minimum <= Configuration.Settings.SubtitleSettings.DCinemaBottomMargin &&
                    numericUpDownTopBottomMargin.Maximum >= Configuration.Settings.SubtitleSettings.DCinemaBottomMargin)
                    numericUpDownTopBottomMargin.Value = Configuration.Settings.SubtitleSettings.DCinemaBottomMargin;
                else
                    numericUpDownTopBottomMargin.Value = 8;

            }
            FixLargeFonts(buttonCancel);
        }

        private void buttonFontColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panelFontColor.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panelFontColor.BackColor = colorDialog1.Color;
            }
        }

        private void buttonFontEffectColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panelFontEffectColor.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panelFontEffectColor.BackColor = colorDialog1.Color;
            }
        }

        private void buttonGenerateID_Click(object sender, EventArgs e)
        {
            string hex = Guid.NewGuid().ToString().Replace("-", string.Empty);
            textBoxSubtitleID.Text = "urn:uuid:" + hex.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
        }

        private void buttonToday_Click(object sender, EventArgs e)
        {
            textBoxIssueDate.Text = DateTime.Now.ToString("s") + ".000-00:00";
        }

        private void buttonOK_Click_1(object sender, EventArgs e)
        {
            var ss = Configuration.Settings.SubtitleSettings;
            ss.CurrentDCinemaSubtitleId = textBoxSubtitleID.Text;
            ss.CurrentDCinemaMovieTitle = textBoxMovieTitle.Text;
            ss.CurrentDCinemaReelNumber = numericUpDownReelNumber.Value.ToString();
            ss.CurrentDCinemaEditRate = textBoxEditRate.Text;
            ss.CurrentDCinemaTimeCodeRate = comboBoxTimeCodeRate.Text;
            ss.CurrentDCinemaStartTime = timeUpDownStartTime.TimeCode.ToHHMMSSFF();
            if (comboBoxLanguage.SelectedItem != null)
                ss.CurrentDCinemaLanguage = comboBoxLanguage.SelectedItem.ToString();
            else
                ss.CurrentDCinemaLanguage = string.Empty;
            ss.CurrentDCinemaIssueDate = textBoxIssueDate.Text;
            ss.CurrentDCinemaFontId = textBoxFontID.Text;
            ss.CurrentDCinemaFontUri = textBoxFontUri.Text;
            ss.CurrentDCinemaFontColor = panelFontColor.BackColor;
            if (comboBoxFontEffect.SelectedIndex == 1)
                ss.CurrentDCinemaFontEffect = "border";
            else if (comboBoxFontEffect.SelectedIndex == 2)
                ss.CurrentDCinemaFontEffect = "shadow";
            else
                ss.CurrentDCinemaFontEffect = string.Empty;
            ss.CurrentDCinemaFontEffectColor = panelFontEffectColor.BackColor;
            ss.CurrentDCinemaFontSize = (int)numericUpDownFontSize.Value;
            Configuration.Settings.SubtitleSettings.DCinemaBottomMargin = (int)numericUpDownTopBottomMargin.Value;

            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string hex = Guid.NewGuid().ToString().Replace("-", string.Empty);
            textBoxFontUri.Text = "urn:uuid:" + hex.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
