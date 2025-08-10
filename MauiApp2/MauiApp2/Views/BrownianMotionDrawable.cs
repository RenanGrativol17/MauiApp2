using MauiApp2.Helpers;

namespace MauiApp2.Views
{
    /// <summary>
    /// Classe IDrawable que renderiza o gráfico de Movimento Browniano em um GraphicsView.
    /// </summary>
    public class BrownianMotionDrawable : BindableObject, IDrawable
    {
        /// <summary>
        /// A lista de arrays de resultados da simulação a serem desenhados.
        /// </summary>
        public static readonly BindableProperty SimulationResultsListProperty =
            BindableProperty.Create(
                nameof(SimulationResultsList),
                typeof(List<double[]>),
                typeof(BrownianMotionDrawable),
                defaultValue: new List<double[]>(),
                propertyChanged: OnSimulationResultsChanged);

        public List<double[]> SimulationResultsList
        {
            get => (List<double[]>)GetValue(SimulationResultsListProperty);
            set => SetValue(SimulationResultsListProperty, value);
        }

        /// <summary>
        /// A cor da linha usada para desenhar as simulações.
        /// </summary>
        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create(
                nameof(LineColor),
                typeof(Color),
                typeof(BrownianMotionDrawable),
                defaultValue: Colors.Blue,
                propertyChanged: OnSimulationResultsChanged);

        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        private static void OnSimulationResultsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BrownianMotionDrawable drawable)
            {
                var graphicsView = drawable.FindParentOfType<GraphicsView>();
                graphicsView?.Invalidate();
            }
        }

        /// <summary>
        /// Desenha o gráfico de Movimento Browniano no canvas.
        /// </summary>
        /// <param name="canvas">O contexto de desenho do canvas.</param>
        /// <param name="dirtyRect">O retângulo de delimitação para o desenho.</param>
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.White;
            canvas.FillRectangle(dirtyRect);

            if (SimulationResultsList == null || SimulationResultsList.Count == 0 || SimulationResultsList.All(x => x == null || x.Length == 0))
            {
                canvas.FontColor = Colors.Gray;
                canvas.DrawString("Execute a simulação", dirtyRect, HorizontalAlignment.Center, VerticalAlignment.Center);
                return;
            }

            double min = SimulationResultsList.SelectMany(x => x).Min();
            double max = SimulationResultsList.SelectMany(x => x).Max();
            float xStep = dirtyRect.Width / (SimulationResultsList.FirstOrDefault()?.Length ?? 1);
            float yScale = dirtyRect.Height / (float)(max - min);

            canvas.StrokeColor = Colors.Black;
            canvas.FontSize = 10;
            const int numLabels = 5;
            float margin = 5;

            for (int i = 0; i <= numLabels; i++)
            {
                float y = dirtyRect.Height - dirtyRect.Height * i / numLabels;
                double price = min + (max - min) * i / numLabels;

                canvas.DrawString($"{price:F2}", new RectF(margin, y - 20, 50, 20), HorizontalAlignment.Left, VerticalAlignment.Center);
                canvas.DrawLine(0, y, margin, y);
            }

            int totalDays = SimulationResultsList.FirstOrDefault()?.Length ?? 0;
            for (int i = 0; i <= numLabels; i++)
            {
                float x = dirtyRect.Width * i / numLabels;
                int day = (int)(totalDays * i / numLabels);

                canvas.DrawString($"{day}", new RectF(x - 25, dirtyRect.Height, 50, margin * 2), HorizontalAlignment.Center, VerticalAlignment.Top);
                canvas.DrawLine(x, dirtyRect.Height, x, dirtyRect.Height - margin);
            }

            canvas.DrawLine(0, dirtyRect.Height, dirtyRect.Width, dirtyRect.Height);
            canvas.DrawLine(0, 0, 0, dirtyRect.Height);

            foreach (var results in SimulationResultsList)
            {
                canvas.StrokeColor = LineColor;
                canvas.StrokeSize = 2;
                var path = new PathF();
                for (int i = 0; i < results.Length; i++)
                {
                    float x = i * xStep;
                    float y = dirtyRect.Height - (float)(results[i] - min) * yScale;

                    if (i == 0) path.MoveTo(x, y);
                    else path.LineTo(x, y);
                }
                canvas.DrawPath(path);
            }
        }
    }
}