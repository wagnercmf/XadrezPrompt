using System;
using xadrez_console.tabuleiro;
using xadrez_console.xadrez;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.Terminada)
                {
                    try
                    {
                            Console.Clear();
                            Tela.imprimirPartida(partida);
                            Console.WriteLine();
                            Console.WriteLine("Origem: ");
                            Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                            //Posicao origem = new PosicaoXadrez('A',1).toPosicao();
                            partida.validarPosicaoOrigem(origem);
                            bool[,] posicoesPossiveis = partida.Tabuleiro.peca(origem).movimentosPossiveis();
                            Console.Clear();
                            Tela.imprimirTabuleiro(partida.Tabuleiro, posicoesPossiveis);
                            Console.WriteLine("\nDestino: ");
                            Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
                            partida.validarPosicaoDestino(origem, destino);
                            partida.realizaJogada(origem, destino);
                    }
                    catch(TabuleiroException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
                Console.Clear();
                Tela.imprimirPartida(partida);
            }
            catch(TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
