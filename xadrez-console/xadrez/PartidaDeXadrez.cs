using System;
using System.Collections.Generic;
using System.Text;
using xadrez_console.tabuleiro;

namespace xadrez_console.xadrez
{
    public class PartidaDeXadrez
    {
        public Tabuleiro Tabuleiro { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }


        public PartidaDeXadrez()
        {
            Tabuleiro = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            colocarPecas();
            VulneravelEnPassant = null;
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tabuleiro.retirarPeca(origem);
            p.incrementarQtdeMovimentos();
            Peca pecaCapturada = Tabuleiro.retirarPeca(destino);
            Tabuleiro.colocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }

            //Roque Pequeno
            if(p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tabuleiro.retirarPeca(origemT);
                T.incrementarQtdeMovimentos();
                Tabuleiro.colocarPeca(T, destinoT);
            }

            //Roque Grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tabuleiro.retirarPeca(origemT);
                T.incrementarQtdeMovimentos();
                Tabuleiro.colocarPeca(T, destinoT);
            }

            //en Passant
            if(p is Peao){
                if(origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if(p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = Tabuleiro.retirarPeca(posP);
                    Capturadas.Add(pecaCapturada);
                }
            }
            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tabuleiro.retirarPeca(destino);
            p.decrementarQtdeMovimentos();
            if(pecaCapturada != null)
            {
                Tabuleiro.colocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }

            Tabuleiro.colocarPeca(p, origem);

            //Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tabuleiro.retirarPeca(destinoT);
                T.decrementarQtdeMovimentos();
                Tabuleiro.colocarPeca(T, origemT);
            }

            //Roque Grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tabuleiro.retirarPeca(destinoT);
                T.decrementarQtdeMovimentos();
                Tabuleiro.colocarPeca(T, origemT);
            }

            //en Passant
            if(p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tabuleiro.retirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tabuleiro.colocarPeca(peao, posP);
                }
            }

        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque!");
            }
            Peca p = Tabuleiro.peca(destino);

            //Promoção
            if(p is Peao)
            {
                if((p.Cor == Cor.Branca && destino.Linha == 0) || (p.Cor == Cor.Preta && destino.Linha == 7)){
                    p = Tabuleiro.retirarPeca(destino);
                    Pecas.Remove(p);
                    Peca queen = new Rainha(Tabuleiro, p.Cor);
                    Tabuleiro.colocarPeca(queen, destino);
                    Pecas.Add(queen);
                }
            }

            if (estaEmXeque(adversaria(JogadorAtual))) Xeque = true;
            else Xeque = false;

            if (testeXequeMate(adversaria(JogadorAtual))) Terminada = true;
            else
            {
                Turno++;
                mudaJogador();
            }

            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2)) VulneravelEnPassant = p;
            else VulneravelEnPassant = null;
        }

        public void validarPosicaoOrigem(Posicao pos)
        {
            if (Tabuleiro.peca(pos) == null) throw new TabuleiroException("Não existe peça na posição escolhida!");
            if (JogadorAtual != Tabuleiro.peca(pos).Cor) throw new TabuleiroException("A peça escolhida não é sua!");
            if (!Tabuleiro.peca(pos).existeMovimentosPossiveis()) throw new TabuleiroException("Não há movimentos possíveis para essa peça!");
        }

        public void validarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tabuleiro.peca(origem).movimentoPossivel(destino)) throw new TabuleiroException("Posição de destino inválida!");
     
        }

        public void mudaJogador()
        {
            if(JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas)
            {
                if (x.Cor == cor) aux.Add(x);
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor) aux.Add(x);
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria (Cor cor)
        {
            if (cor == Cor.Branca) return Cor.Preta;
            else return Cor.Branca;
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei) return x;
            }
            return null;
        }

        private bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null) throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro");
            foreach(Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if(mat[R.Posicao.Linha, R.Posicao.Coluna] == true) return true;
            }
            return false;
        }       

        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor)) return false;
            foreach(Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for(int i = 0; i < Tabuleiro.NumLinhas; i++)
                {
                    for(int j = 0; j < Tabuleiro.NumColunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) return false;
                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tabuleiro.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            Pecas.Add(peca);
        }

        public void colocarPecas()
        {
            colocarNovaPeca('A', 1 ,new Torre(Tabuleiro, Cor.Branca));
            colocarNovaPeca('B', 1, new Cavalo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('C', 1, new Bispo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('D', 1, new Rainha(Tabuleiro, Cor.Branca));
            colocarNovaPeca('E', 1, new Rei(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('F', 1, new Bispo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('G', 1, new Cavalo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('H', 1, new Torre(Tabuleiro, Cor.Branca));
            colocarNovaPeca('A', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('B', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('C', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('D', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('E', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('F', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('G', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('H', 2, new Peao(Tabuleiro, Cor.Branca, this));

            colocarNovaPeca('A', 8, new Torre(Tabuleiro, Cor.Preta));
            colocarNovaPeca('B', 8, new Cavalo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('C', 8, new Bispo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('D', 8, new Rainha(Tabuleiro, Cor.Preta));
            colocarNovaPeca('E', 8, new Rei(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('F', 8, new Bispo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('G', 8, new Cavalo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('H', 8, new Torre(Tabuleiro, Cor.Preta));
            colocarNovaPeca('A', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('B', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('C', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('D', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('E', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('F', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('G', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('H', 7, new Peao(Tabuleiro, Cor.Preta, this));
        }
    }
}
