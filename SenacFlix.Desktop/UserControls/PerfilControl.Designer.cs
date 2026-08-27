namespace SenacFlix.Desktop.UserControls
{
    partial class PerfilControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblNomeInfo = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.lblEmailInfo = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPerfisInfo = new System.Windows.Forms.Label();
            this.lblPerfis = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(175, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Meu Perfil";
            // 
            // lblNomeInfo
            // 
            this.lblNomeInfo.AutoSize = true;
            this.lblNomeInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblNomeInfo.ForeColor = System.Drawing.Color.DarkGray;
            this.lblNomeInfo.Location = new System.Drawing.Point(34, 110);
            this.lblNomeInfo.Name = "lblNomeInfo";
            this.lblNomeInfo.Size = new System.Drawing.Size(61, 21);
            this.lblNomeInfo.TabIndex = 1;
            this.lblNomeInfo.Text = "Nome:";
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblNome.ForeColor = System.Drawing.Color.White;
            this.lblNome.Location = new System.Drawing.Point(33, 140);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(161, 30);
            this.lblNome.TabIndex = 2;
            this.lblNome.Text = "Nome do Usuário";
            // 
            // lblEmailInfo
            // 
            this.lblEmailInfo.AutoSize = true;
            this.lblEmailInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblEmailInfo.ForeColor = System.Drawing.Color.DarkGray;
            this.lblEmailInfo.Location = new System.Drawing.Point(34, 190);
            this.lblEmailInfo.Name = "lblEmailInfo";
            this.lblEmailInfo.Size = new System.Drawing.Size(63, 21);
            this.lblEmailInfo.TabIndex = 3;
            this.lblEmailInfo.Text = "E-mail:";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblEmail.ForeColor = System.Drawing.Color.White;
            this.lblEmail.Location = new System.Drawing.Point(33, 220);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(155, 30);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "email@teste.com";
            // 
            // lblPerfisInfo
            // 
            this.lblPerfisInfo.AutoSize = true;
            this.lblPerfisInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPerfisInfo.ForeColor = System.Drawing.Color.DarkGray;
            this.lblPerfisInfo.Location = new System.Drawing.Point(34, 270);
            this.lblPerfisInfo.Name = "lblPerfisInfo";
            this.lblPerfisInfo.Size = new System.Drawing.Size(57, 21);
            this.lblPerfisInfo.TabIndex = 5;
            this.lblPerfisInfo.Text = "Perfis:";
            // 
            // lblPerfis
            // 
            this.lblPerfis.AutoSize = true;
            this.lblPerfis.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblPerfis.ForeColor = System.Drawing.Color.White;
            this.lblPerfis.Location = new System.Drawing.Point(33, 300);
            this.lblPerfis.Name = "lblPerfis";
            this.lblPerfis.Size = new System.Drawing.Size(78, 30);
            this.lblPerfis.TabIndex = 6;
            this.lblPerfis.Text = "Admin, Operador";
            // 
            // PerfilControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(14)))));
            this.Controls.Add(this.lblPerfis);
            this.Controls.Add(this.lblPerfisInfo);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblEmailInfo);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.lblNomeInfo);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "PerfilControl";
            this.Size = new System.Drawing.Size(1030, 720);
            this.Load += new System.EventHandler(this.PerfilControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNomeInfo;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Label lblEmailInfo;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPerfisInfo;
        private System.Windows.Forms.Label lblPerfis;
    }
}
