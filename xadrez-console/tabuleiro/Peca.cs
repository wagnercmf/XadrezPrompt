using System;
using System.Collections.Generic;
using System.Text;

namespace xadrez_console.tabuleiro
{
    public abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QteMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Tabuleiro tab, Cor cor)
        {
            Posicao = null;
            Cor = cor;
            Tab = tab;
            QteMovimentos = 0;
        }

        public void incrementarQtdeMovimentos()
        {
            QteMovimentos++;
        }

        public void decrementarQtdeMovimentos()
        {
            QteMovimentos--;
        }

        public bool existeMovimentosPossiveis()     //testar esse depois
        {
            bool[,] mat = movimentosPossiveis();
            for(int i = 0; i < Tab.NumLinhas; i++)
            {
                for (int j = 0; j < Tab.NumColunas; j++)
                {
                    if (mat[i, j]) return true;
                }
                    
            }
            return false;
        }

        public bool movimentoPossivel(Posicao pos)
        {
            return movimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] movimentosPossiveis();        
    }
}
