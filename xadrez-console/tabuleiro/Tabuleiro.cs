using System;
using System.Collections.Generic;
using System.Text;

namespace xadrez_console.tabuleiro
{
    public class Tabuleiro
    {
        public int NumLinhas { get; set; }
        public int NumColunas { get; set; }
        private Peca[,] Pecas;

        public Tabuleiro(int numLinhas, int numColunas)
        {
            NumLinhas = numLinhas;
            NumColunas = numColunas;
            Pecas = new Peca[numLinhas, numColunas];
        }

        public Peca peca(int linha, int coluna)
        {
            return Pecas[linha, coluna];
        }

        public Peca peca(Posicao pos)
        {
            return Pecas[pos.Linha, pos.Coluna];
        }

        public bool existePeca(Posicao pos)
        {
            validarPosicao(pos);
            return peca(pos) != null;
        }

        public void colocarPeca(Peca p, Posicao pos)
        {
            if (existePeca(pos))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição");
            }
            Pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }

        public Peca retirarPeca(Posicao pos)
        {
            if(peca(pos) == null)
            {
                return null;
            }
            Peca aux = peca(pos);
            aux.Posicao = null;
            Pecas[pos.Linha, pos.Coluna] = null;
            return aux;
        }

        public bool posicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Linha >= NumLinhas || pos.Coluna < 0 || pos.Coluna >= NumColunas)
            {
                return false;
            }
            else return true;
        }

        public void validarPosicao(Posicao pos)
        {
            if (!posicaoValida(pos))
            {
                throw new TabuleiroException("Posição Inválida!");
            }
        }

    }
}
