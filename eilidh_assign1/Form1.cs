///////*
//////    Eilidh Macsymic
//////    Assignment 1 - Creating a game

//////    Used as references:
//////        https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-windows-forms-create-match-game?view=vs-2022
//////        https://github.com/BetulTugce/MatchingGame
//////        https://github.com/achyuthkp/Memory-Matching-Game/tree/master
////// */

namespace eilidh_assign1
{
    public partial class Form1 : Form
    {
        private List<Button> buttons;
        private List<Image> images;
        private Button firstClicked, secondClicked;
        private int matchesFound, movement;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();

            this.btn_NewGame.Click += new System.EventHandler(this.btn_NewGame_Click);
        }

        private void btn_NewGame_Click(object sender, EventArgs e)
        {
            matchesFound = 0;
            movement = 0;
            tableLayoutPanel1.Enabled = true;
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Button btn)
                {
                    btn.Visible = true;
                    btn.Enabled = true;
                    btn.BackgroundImage = null;
                }
                InitializeGame();
            }
        }

        private void InitializeGame()
        {
            buttons = new List<Button>();
            images = new List<Image>
            {
                Properties.Resources.allium,
                Properties.Resources.azure_bluet,
                Properties.Resources.blue_orchid,
                Properties.Resources.fern,
                Properties.Resources.lily_of_the_valley,
                Properties.Resources.orange_tulip,
                Properties.Resources.oxeye_daisy,
                Properties.Resources.poppy,
                Properties.Resources.red_tulip,
                Properties.Resources.wither_rose
            };

            // Duplicate images to create pairs
            images.AddRange(images);

            // Shuffle images
            Random rand = new Random();
            images = images.OrderBy(x => rand.Next()).ToList();

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Button button)
                {
                    button.BackgroundImage = null;
                    button.Tag = null; // Clear tag for future use
                    button.Click += btnFlower_Click; // Ensure this is only added once
                    buttons.Add(button);
                }
            }

            matchesFound = 0;
            movement = 0;
            tableLayoutPanel1.Enabled = true;
            btn_NewGame.Enabled = true;
        }

        private void btnFlower_Click(object sender, EventArgs e)
        {
            if (!(sender is Button clickedButton) || clickedButton.BackgroundImage != null)
                return;

            int index = buttons.IndexOf(clickedButton);
            clickedButton.BackgroundImage = images[index];
            clickedButton.BackgroundImageLayout = ImageLayout.Stretch;

            if (firstClicked == null)
            {
                firstClicked = clickedButton;
                return;
            }

            secondClicked = clickedButton;
            CheckForMatch();
        }

        private async void CheckForMatch()
        {
            if (firstClicked.BackgroundImage == secondClicked.BackgroundImage)
            {
                matchesFound++;
                firstClicked.Enabled = false;
                secondClicked.Enabled = false;
                await Task.Delay(1000);
                firstClicked.Visible = false;
                secondClicked.Visible = false;
            }
            else
            {
                await Task.Delay(1000);
                firstClicked.BackgroundImage = null;
                secondClicked.BackgroundImage = null;
            }

            movement++;

            firstClicked = null;
            secondClicked = null;

            // Check for win
            if (matchesFound == 10)
            {
                MessageBox.Show($"Congratulations! You beat the game in {movement} moves!");
                InitializeGame();
            }
        }

        private void btn_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Memory Match, also known as Concentration, is a card game where players aim to find pairs of matching cards. " +
                "The game consists of a grid of face-down cards, each hiding an image or symbol. " +
                "Players take turns flipping over two cards at a time. If the cards match, the player keeps the pair and gets another turn; " +
                "if not, the cards are flipped back over. The game continues until all pairs are found, and the player with the most pairs wins. " +
                "It's a fun way to enhance memory and concentration skills and is suitable for all ages!");
        }
    }
}
