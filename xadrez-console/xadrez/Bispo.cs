using System;
using System.Collections.Generic;
using System.Text;
using xadrez_console.tabuleiro;

namespace xadrez_console.xadrez
{
    public class Bispo : Peca
    {
        public Bispo(Tabuleiro tab, Cor cor) : base(tab, cor)
        {

        }

        public override string ToString()
        {
            return "B";
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return (p == null || p.Cor != this.Cor);
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.NumLinhas, Tab.NumColunas];

            Posicao pos = new Posicao(0, 0);

            //Noroeste
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor) break;
                pos.definirValores(pos.Linha - 1, pos.Coluna - 1);
            }
            //Nordeste
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor) break;
                pos.definirValores(pos.Linha - 1, pos.Coluna + 1);
            }
            //Sudeste
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor) break;
                pos.definirValores(pos.Linha + 1, pos.Coluna + 1);
            }
            //Sudoste
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor) break;
                pos.definirValores(pos.Linha + 1, pos.Coluna - 1);
            }
            return mat;
        }
    }
}
