using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Useful
{
    public class TimerRequest
    {
        public TimerRequest(int totalRequisicaoPermitido, int tempoRequisicaoPermitido)
        {
            this.SetTemporizador(totalRequisicaoPermitido, tempoRequisicaoPermitido);
        }

        private int _RequisicaoAtual { get; set; }

        private DateTime _TimeStart { get; set; } = DateTime.Now;

        private int _TotalRequisicaoPermitido { get; set; }

        private int _TempoRequisicaoPermitido { get; set; }

        public void SetTemporizador(int totalRequisicaoPermitido, int tempoRequisicaoPermitido)
        {
            this._TotalRequisicaoPermitido = totalRequisicaoPermitido;
            this._TempoRequisicaoPermitido = tempoRequisicaoPermitido;
        }

        public void VerificaTemporizador()
        {
            ++this._RequisicaoAtual;
            if (this._RequisicaoAtual >= this._TotalRequisicaoPermitido)
            {
                while (this._TempoRequisicaoPermitido >= this.GetDiffIntervalo());
            }
            this.VerificaIntervaloRequisicao();
        }

        private void VerificaIntervaloRequisicao()
        {
            if (this.GetDiffIntervalo() < this._TempoRequisicaoPermitido)
                return;
            this._TimeStart = DateTime.Now;
            this._RequisicaoAtual = 1;
        }

        private int GetDiffIntervalo()
        {
            return Convert.ToInt32((DateTime.Now - this._TimeStart).TotalMilliseconds);
        }
    }
}
