using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using XIVLauncher.Common.Dalamud;

namespace Dalamud.Updater
{
    internal class DalamudLoadingOverlay : IDalamudLoadingOverlay
    {
        public delegate void progressBar(int value);
        public delegate void statusLabel(string value);
        public delegate void setVisible(bool value);
        public event progressBar OnProgressBar;
        public event statusLabel OnStatusLabel;
        public event setVisible OnSetVisible;
        public DalamudLoadingOverlay(FormMain form)
        {
            //this.progressBar = form.toolStripProgressBar1;
            //this.statusLabel = form.toolStripStatusLabel1;
        }
        public void ReportProgress(long? size, long downloaded, double? progress)
        {
            size = size ?? 0;
            progress = progress ?? 0;
            OnProgressBar?.Invoke((int)progress.Value);
        }

        public void SetInvisible()
        {
            OnSetVisible?.Invoke(false);
        }

        public void SetStep(IDalamudLoadingOverlay.DalamudUpdateStep progress)
        {
            switch (progress)
            {
                // 너무 길면 아예 표시 안됨
                case IDalamudLoadingOverlay.DalamudUpdateStep.Dalamud:
                    OnStatusLabel?.Invoke("Dalamud...");
                    break;

                case IDalamudLoadingOverlay.DalamudUpdateStep.Assets:
                    OnStatusLabel?.Invoke("Assets...");
                    break;

                case IDalamudLoadingOverlay.DalamudUpdateStep.Runtime:
                    OnStatusLabel?.Invoke("Runtime...");
                    break;

                case IDalamudLoadingOverlay.DalamudUpdateStep.Unavailable:
                    OnStatusLabel?.Invoke("사용불가");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(progress), progress, null);
            }
        }

        public void SetVisible()
        {
            OnSetVisible?.Invoke(true);
        }
    }
}
