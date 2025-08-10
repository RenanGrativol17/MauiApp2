using MauiApp2.Models;
using MauiApp2.Models.MauiApp2.Models;
using MauiApp2.Views;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp2.ViewModels
{
    /// <summary>
    /// ViewModel para a simulação do Movimento Browniano, gerenciando a lógica e o estado da UI.
    /// </summary>
    public class BrownianMotionViewModel : INotifyPropertyChanged
    {
        private double _initialPrice = 100;
        private double _volatility = 0.20;
        private double _meanReturn = 0.01;
        private int _timeDaysValue = 252; // Valor numérico validado para uso na lógica
        private string _timeDaysText = "252"; // Entrada de texto do usuário (bind TwoWay)
        private string _timeDaysError; // Mensagem de erro para validação
        private int _numberOfSimulations = 5;

        /// <summary>
        /// Coleção de opções de cores para o seletor.
        /// </summary>
        private List<ColorOption> _colorOptions;
        public List<ColorOption> ColorOptions
        {
            get => _colorOptions;
            set => SetProperty(ref _colorOptions, value);
        }

        /// <summary>
        /// A opção de cor selecionada pelo usuário.
        /// </summary>
        private ColorOption _selectedColor;
        public ColorOption SelectedColor
        {
            get => _selectedColor;
            set
            {
                SetProperty(ref _selectedColor, value);
                // Atualiza a cor do drawable se a opção selecionada for válida
                if (Drawable != null && value != null)
                {
                    Drawable.LineColor = value.Value;
                }
            }
        }

        private BrownianMotionDrawable _drawable = new();

        /// <summary>
        /// O objeto Drawable responsável por desenhar o gráfico.
        /// </summary>
        public BrownianMotionDrawable Drawable
        {
            get => _drawable;
            set => SetProperty(ref _drawable, value);
        }

        /// <summary>
        /// O preço inicial do ativo para a simulação.
        /// </summary>
        public double InitialPrice
        {
            get => _initialPrice;
            set => SetProperty(ref _initialPrice, value);
        }

        /// <summary>
        /// A volatilidade do ativo.
        /// </summary>
        public double Volatility
        {
            get => _volatility;
            set => SetProperty(ref _volatility, value);
        }

        /// <summary>
        /// O retorno médio diário do ativo.
        /// </summary>
        public double MeanReturn
        {
            get => _meanReturn;
            set => SetProperty(ref _meanReturn, value);
        }

        /// <summary>
        /// A entrada de texto para o número de dias da simulação. Inclui validação.
        /// </summary>
        public string TimeDaysText
        {
            get => _timeDaysText;
            set
            {
                if (SetProperty(ref _timeDaysText, value))
                {
                    ValidateTimeDays();
                }
            }
        }

        /// <summary>
        /// Mensagem de erro para a validação do campo de dias.
        /// </summary>
        public string TimeDaysError
        {
            get => _timeDaysError;
            set => SetProperty(ref _timeDaysError, value);
        }

        /// <summary>
        /// O valor validado do número de dias, usado para a simulação.
        /// </summary>
        public int TimeDays
        {
            get => _timeDaysValue;
            set => SetProperty(ref _timeDaysValue, value);
        }

        /// <summary>
        /// O número de simulações a serem executadas.
        /// </summary>
        public int NumberOfSimulations
        {
            get => _numberOfSimulations;
            set => SetProperty(ref _numberOfSimulations, value);
        }

        /// <summary>
        /// Comando para iniciar a simulação.
        /// </summary>
        public ICommand RunSimulationCommand { get; }

        public BrownianMotionViewModel()
        {
            ColorOptions = new List<ColorOption>
            {
                new ColorOption { Name = "Azul", Value = Color.FromHex("#512BD4") },
                new ColorOption { Name = "Verde", Value = Color.FromHex("#008000") },
                new ColorOption { Name = "Vermelho", Value = Color.FromHex("#FF0000") },
                new ColorOption { Name = "Dourado", Value = Color.FromHex("#FFD700") }
            };

            SelectedColor = ColorOptions.FirstOrDefault();

            RunSimulationCommand = new Command(ExecuteRunSimulation);
        }

        /// <summary>
        /// Valida a entrada de texto do campo TimeDaysText.
        /// </summary>
        private void ValidateTimeDays()
        {
            if (int.TryParse(TimeDaysText, out int result))
            {
                if (result > 0 && result <= 365)
                {
                    TimeDays = result;
                    TimeDaysError = null; // Limpa o erro
                }
                else
                {
                    TimeDaysError = "O valor deve ser entre 1 e 365.";
                }
            }
            else
            {
                TimeDaysError = "Entrada inválida. Por favor, insira um número.";
            }
        }

        /// <summary>
        /// Executa a simulação e atualiza o objeto Drawable com os novos resultados.
        /// </summary>
        private void ExecuteRunSimulation()
        {
            // Valida antes de executar, para garantir que o valor é válido
            ValidateTimeDays();
            if (!string.IsNullOrEmpty(TimeDaysError))
            {
                return; // Impede a execução se houver um erro
            }

            try
            {
                var allResults = new List<double[]>();
                for (int i = 0; i < NumberOfSimulations; i++)
                {
                    var results = BrownianMotionModel.GenerateBrownianMotion(
                        Volatility,
                        MeanReturn,
                        InitialPrice,
                        TimeDays);
                    allResults.Add(results);
                }

                Drawable = new BrownianMotionDrawable
                {
                    SimulationResultsList = allResults,
                    LineColor = SelectedColor.Value
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}