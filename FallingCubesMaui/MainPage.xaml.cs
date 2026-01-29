using System.Timers;

namespace FallingCubesMaui;

public partial class MainPage : ContentPage
{
    const int Rows = 20;
    const int Cols = 10;

    readonly int[,] board = new int[Rows, Cols];

    readonly BoxView[,] cells = new BoxView[Rows, Cols];

    int currentRow;
    int currentCol;

    int score = 0;

    readonly System.Timers.Timer timer;

    bool running = false;

    public MainPage()
    {
        InitializeComponent();

        BuildBoardUI();

        timer = new System.Timers.Timer(350); 
        timer.Elapsed += (_, _) =>
        {
           
            MainThread.BeginInvokeOnMainThread(GameTick);
        };
    }

    void BuildBoardUI()
    {
        BoardGrid.RowDefinitions.Clear();
        BoardGrid.ColumnDefinitions.Clear();
        BoardGrid.Children.Clear();

        for (int r = 0; r < Rows; r++)
            BoardGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

        for (int c = 0; c < Cols; c++)
            BoardGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                var box = new BoxView { Color = Colors.Transparent };

                var frame = new Frame
                {
                    Padding = 0,
                    Margin = 1,
                    BorderColor = Color.FromArgb("#2C2C2C"),
                    BackgroundColor = Color.FromArgb("#141414"),
                    Content = box,
                    CornerRadius = 4,
                    HasShadow = false
                };

                cells[r, c] = box;

                BoardGrid.Add(frame, c, r);
            }
        }

        Redraw();
    }

    void Redraw()
    {

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (board[r, c] == 1)
                    cells[r, c].Color = Color.FromArgb("#2D7DFF"); 
                else
                    cells[r, c].Color = Colors.Transparent;       
            }
        }

        if (running)
            cells[currentRow, currentCol].Color = Color.FromArgb("#FF3B30"); 

        ScoreLabel.Text = $"Score: {score}";
    }

    void OnStartClicked(object sender, EventArgs e)
    {
        if (running) return;

        running = true;
        SpawnNewCube();
        timer.Start();
        Redraw();
    }

    void OnResetClicked(object sender, EventArgs e)
    {
        timer.Stop();
        running = false;
        score = 0;

        Array.Clear(board, 0, board.Length);

       
        currentRow = 0;
        currentCol = Cols / 2;

        Redraw();
    }

    void GameTick()
    {
        if (!running) return;

        if (CanMove(currentRow + 1, currentCol))
        {
            currentRow++;
            Redraw();
            return;
        }

     
        board[currentRow, currentCol] = 1;
        score += 10;

       
        SpawnNewCube();

      
        if (!CanMove(currentRow, currentCol))
        {
            running = false;
            timer.Stop();
            DisplayAlert("Game Over", "Se llenó el tablero.", "OK");
        }

        Redraw();
    }


    bool CanMove(int newRow, int newCol)
    {
     
        if (newRow < 0 || newRow >= Rows) return false;
        if (newCol < 0 || newCol >= Cols) return false;

        if (board[newRow, newCol] == 1) return false;

        return true;
    }

 
    void SpawnNewCube()
    {
        currentRow = 0;
        currentCol = Cols / 2;
    }

    void OnLeftClicked(object sender, EventArgs e)
    {
        if (!running) return;

        if (CanMove(currentRow, currentCol - 1))
        {
            currentCol--;
            Redraw();
        }
    }

    void OnRightClicked(object sender, EventArgs e)
    {
        if (!running) return;

        if (CanMove(currentRow, currentCol + 1))
        {
            currentCol++;
            Redraw();
        }
    }

    void OnDownClicked(object sender, EventArgs e)
    {
        if (!running) return;

        
        if (CanMove(currentRow + 1, currentCol))
        {
            currentRow++;
            Redraw();
        }
    }
}
