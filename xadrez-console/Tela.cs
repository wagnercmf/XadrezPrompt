using System;
using System.Collections.Generic;
using xadrez_console.tabuleiro;
using xadrez_console.xadrez;

namespace xadrez_console
{
    public class Tela
    {
        public static void imprimirPartida(PartidaDeXadrez partida)
        {
            Tela.imprimirTabuleiro(partida.Tabuleiro);
            imprimirPecasCapturadas(partida);
            Console.WriteLine("\n\nTurno: " + partida.Turno);
            if (!partida.Terminada)
            {
                Console.WriteLine("\nAguardando jogada da peça: " + partida.JogadorAtual);
                if (partida.Xeque)
                {
                    Console.Write("\nXEQUE MULEKE!");
                }
            }
            else
            {
                Console.WriteLine("\nXEQUEMATE HIAHIAHIA");
                Console.WriteLine("The winner is... " + partida.JogadorAtual);
            }
        }

        public static void imprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("\nPeças Capturadas:");
            Console.Write("Brancas: ");
            imprimirConjunto(partida.pecasCapturadas(Cor.Branca));
            Console.Write("\tPretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            imprimirConjunto(partida.pecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;
        }

        public static void imprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            foreach(Peca x in conjunto)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        public static void imprimirTabuleiro(Tabuleiro tab)
        {
            for(int i = 0; i < tab.NumLinhas; i++)
            {
                Console.Write(8 - i + " " );
                for (int j = 0; j < tab.NumLinhas; j++)
                {                    
                     Tela.imprimirPeca(tab.peca(i, j));
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
        }

        public static void imprimirTabuleiro(Tabuleiro tab, bool [,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkMagenta;

            for (int i = 0; i < tab.NumLinhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.NumLinhas; j++)
                {
                    if (posicoesPossiveis[i, j])    Console.BackgroundColor = fundoAlterado;
                    else    Console.BackgroundColor = fundoOriginal;
                    Tela.imprimirPeca(tab.peca(i, j));
                }
                Console.BackgroundColor = fundoOriginal;
                Console.BackgroundColor = fundoOriginal;
                Console.WriteLine();
            }
            Console.BackgroundColor = fundoOriginal;
            Console.WriteLine("  A B C D E F G H");
        }

        public static PosicaoXadrez lerPosicaoXadrez()
        {
            string aux = Console.ReadLine();
            char coluna = aux[0];
            int linha = int.Parse(aux[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }

        public static void imprimirPeca(Peca peca)
        {
            if(peca == null)
            {
                Console.Write("- ");

            }
            else
            { 
                if(peca.Cor == Cor.Branca)
                {
                    Console.Write(peca);
                    Console.Write(" ");
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkCyan ;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                    Console.Write(" ");
                }
            }
        }
    }
}
