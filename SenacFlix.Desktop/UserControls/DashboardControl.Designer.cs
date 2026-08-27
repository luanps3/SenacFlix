namespace SenacFlix.Desktop.UserControls
{
    partial class DashboardControl
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlCardFilmes = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotalFilmes = new System.Windows.Forms.Label();
            this.lblTituloFilmes = new System.Windows.Forms.Label();
            this.pnlCardCategorias = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotalCategorias = new System.Windows.Forms.Label();
            this.lblTituloCategorias = new System.Windows.Forms.Label();
            this.pnlCardUsuarios = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotalUsuarios = new System.Windows.Forms.Label();
            this.lblTituloUsuarios = new System.Windows.Forms.Label();
            this.pnlCardFilmes.SuspendLayout();
            this.pnlCardCategorias.SuspendLayout();
            this.pnlCardUsuarios.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(182, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Dashboard";
            // 
            // pnlCardFilmes
            // 
            this.pnlCardFilmes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.pnlCardFilmes.Controls.Add(this.lblTotalFilmes);
            this.pnlCardFilmes.Controls.Add(this.lblTituloFilmes);
            this.pnlCardFilmes.Location = new System.Drawing.Point(38, 100);
            this.pnlCardFilmes.Name = "pnlCardFilmes";
            this.pnlCardFilmes.Size = new System.Drawing.Size(200, 120);
            this.pnlCardFilmes.TabIndex = 1;
            // 
            // lblTotalFilmes
            // 
            this.lblTotalFilmes.AutoSize = true;
            this.lblTotalFilmes.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTotalFilmes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(159)))), ((int)(((byte)(0)))));
            this.lblTotalFilmes.Location = new System.Drawing.Point(20, 50);
            this.lblTotalFilmes.Name = "lblTotalFilmes";
            this.lblTotalFilmes.Size = new System.Drawing.Size(47, 45);
            this.lblTotalFilmes.TabIndex = 1;
            this.lblTotalFilmes.Text = "...";
            // 
            // lblTituloFilmes
            // 
            this.lblTituloFilmes.AutoSize = true;
            this.lblTituloFilmes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloFilmes.ForeColor = System.Drawing.Color.DarkGray;
            this.lblTituloFilmes.Location = new System.Drawing.Point(20, 20);
            this.lblTituloFilmes.Name = "lblTituloFilmes";
            this.lblTituloFilmes.Size = new System.Drawing.Size(126, 21);
            this.lblTituloFilmes.TabIndex = 0;
            this.lblTituloFilmes.Text = "Total de Filmes";
            // 
            // pnlCardCategorias
            // 
            this.pnlCardCategorias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.pnlCardCategorias.Controls.Add(this.lblTotalCategorias);
            this.pnlCardCategorias.Controls.Add(this.lblTituloCategorias);
            this.pnlCardCategorias.Location = new System.Drawing.Point(260, 100);
            this.pnlCardCategorias.Name = "pnlCardCategorias";
            this.pnlCardCategorias.Size = new System.Drawing.Size(200, 120);
            this.pnlCardCategorias.TabIndex = 2;
            // 
            // lblTotalCategorias
            // 
            this.lblTotalCategorias.AutoSize = true;
            this.lblTotalCategorias.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTotalCategorias.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.lblTotalCategorias.Location = new System.Drawing.Point(20, 50);
            this.lblTotalCategorias.Name = "lblTotalCategorias";
            this.lblTotalCategorias.Size = new System.Drawing.Size(47, 45);
            this.lblTotalCategorias.TabIndex = 1;
            this.lblTotalCategorias.Text = "...";
            // 
            // lblTituloCategorias
            // 
            this.lblTituloCategorias.AutoSize = true;
            this.lblTituloCategorias.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloCategorias.ForeColor = System.Drawing.Color.DarkGray;
            this.lblTituloCategorias.Location = new System.Drawing.Point(20, 20);
            this.lblTituloCategorias.Name = "lblTituloCategorias";
            this.lblTituloCategorias.Size = new System.Drawing.Size(155, 21);
            this.lblTituloCategorias.TabIndex = 0;
            this.lblTituloCategorias.Text = "Total de Categorias";
            // 
            // pnlCardUsuarios
            // 
            this.pnlCardUsuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.pnlCardUsuarios.Controls.Add(this.lblTotalUsuarios);
            this.pnlCardUsuarios.Controls.Add(this.lblTituloUsuarios);
            this.pnlCardUsuarios.Location = new System.Drawing.Point(480, 100);
            this.pnlCardUsuarios.Name = "pnlCardUsuarios";
            this.pnlCardUsuarios.Size = new System.Drawing.Size(200, 120);
            this.pnlCardUsuarios.TabIndex = 3;
            // 
            // lblTotalUsuarios
            // 
            this.lblTotalUsuarios.AutoSize = true;
            this.lblTotalUsuarios.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTotalUsuarios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(100)))));
            this.lblTotalUsuarios.Location = new System.Drawing.Point(20, 50);
            this.lblTotalUsuarios.Name = "lblTotalUsuarios";
            this.lblTotalUsuarios.Size = new System.Drawing.Size(47, 45);
            this.lblTotalUsuarios.TabIndex = 1;
            this.lblTotalUsuarios.Text = "...";
            // 
            // lblTituloUsuarios
            // 
            this.lblTituloUsuarios.AutoSize = true;
            this.lblTituloUsuarios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloUsuarios.ForeColor = System.Drawing.Color.DarkGray;
            this.lblTituloUsuarios.Location = new System.Drawing.Point(20, 20);
            this.lblTituloUsuarios.Name = "lblTituloUsuarios";
            this.lblTituloUsuarios.Size = new System.Drawing.Size(142, 21);
            this.lblTituloUsuarios.TabIndex = 0;
            this.lblTituloUsuarios.Text = "Total de Usuários";
            // 
            // DashboardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(14)))));
            this.Controls.Add(this.pnlCardUsuarios);
            this.Controls.Add(this.pnlCardCategorias);
            this.Controls.Add(this.pnlCardFilmes);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(1030, 720);
            this.Load += new System.EventHandler(this.DashboardControl_Load);
            this.pnlCardFilmes.ResumeLayout(false);
            this.pnlCardFilmes.PerformLayout();
            this.pnlCardCategorias.ResumeLayout(false);
            this.pnlCardCategorias.PerformLayout();
            this.pnlCardUsuarios.ResumeLayout(false);
            this.pnlCardUsuarios.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlCardFilmes;
        private System.Windows.Forms.Label lblTotalFilmes;
        private System.Windows.Forms.Label lblTituloFilmes;
        private Guna.UI2.WinForms.Guna2Panel pnlCardCategorias;
        private System.Windows.Forms.Label lblTotalCategorias;
        private System.Windows.Forms.Label lblTituloCategorias;
        private Guna.UI2.WinForms.Guna2Panel pnlCardUsuarios;
        private System.Windows.Forms.Label lblTotalUsuarios;
        private System.Windows.Forms.Label lblTituloUsuarios;
    }
}
