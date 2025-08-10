namespace MauiApp2.Models
{
    /// <summary>
    /// Fornece métodos estáticos para gerar uma série temporal usando o modelo de Movimento Browniano Geométrico.
    /// </summary>
    public static class BrownianMotionModel
    {
        /// <summary>
        /// Gera uma série de preços simulados com base no modelo de Movimento Browniano Geométrico.
        /// </summary>
        /// <param name="sigma">A volatilidade (desvio padrão do retorno diário) do ativo.</param>
        /// <param name="mean">O retorno médio diário esperado do ativo.</param>
        /// <param name="initialPrice">O preço inicial do ativo para a simulação.</param>
        /// <param name="numDays">O número de dias a serem simulados.</param>
        /// <returns>Um array de <see cref="double"/> contendo os preços simulados para cada dia.</returns>
        public static double[] GenerateBrownianMotion(double sigma, double mean, double initialPrice, int numDays)
        {
            Random rand = new();
            double[] prices = new double[numDays];
            prices[0] = initialPrice;

            for (int i = 1; i < numDays; i++)
            {
                double u1 = 1.0 - rand.NextDouble();
                double u2 = 1.0 - rand.NextDouble();
                double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

                double retornoDiario = mean + sigma * z;

                prices[i] = prices[i - 1] * Math.Exp(retornoDiario);
            }

            return prices;
        }
    }
}