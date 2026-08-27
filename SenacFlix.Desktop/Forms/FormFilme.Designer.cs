namespace SenacFlix.Desktop.Forms
{
    partial class FormFilme
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
            this.txtTitulo = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtDescricao = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtAno = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtDuracao = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtDiretor = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtElenco = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtCategoriaId = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtClassificacaoId = new Guna.UI2.WinForms.Guna2TextBox();
            
            this.chkAtivo = new Guna.UI2.WinForms.Guna2CheckBox();
            this.chkDestaqueHome = new Guna.UI2.WinForms.Guna2CheckBox();
            
            this.txtUrlCapa = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnUploadCapa = new Guna.UI2.WinForms.Guna2Button();
            this.picPreviewCapa = new Guna.UI2.WinForms.Guna2PictureBox();
            
            this.txtUrlBanner = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnUploadBanner = new Guna.UI2.WinForms.Guna2Button();
            this.picPreviewBanner = new Guna.UI2.WinForms.Guna2PictureBox();
            
            this.txtTrailerUrl = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnVisualizarTrailer = new Guna.UI2.WinForms.Guna2Button();
            
            this.txtVideoUrl = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnVisualizarFilme = new Guna.UI2.WinForms.Guna2Button();
            
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.picPreviewCapa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreviewBanner)).BeginInit();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(202, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Detalhes Filme";
            
            // txtTitulo
            this.txtTitulo.Location = new System.Drawing.Point(30, 80);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.PlaceholderText = "Título do Filme";
            this.txtTitulo.Size = new System.Drawing.Size(400, 36);
            this.txtTitulo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtTitulo.ForeColor = System.Drawing.Color.White;
            
            // txtDescricao
            this.txtDescricao.Location = new System.Drawing.Point(30, 130);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.PlaceholderText = "Descrição/Sinopse";
            this.txtDescricao.Size = new System.Drawing.Size(400, 36);
            this.txtDescricao.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtDescricao.ForeColor = System.Drawing.Color.White;
            
            // txtAno
            this.txtAno.Location = new System.Drawing.Point(30, 180);
            this.txtAno.Name = "txtAno";
            this.txtAno.PlaceholderText = "Ano";
            this.txtAno.Size = new System.Drawing.Size(190, 36);
            this.txtAno.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtAno.ForeColor = System.Drawing.Color.White;
            
            // txtDuracao
            this.txtDuracao.Location = new System.Drawing.Point(240, 180);
            this.txtDuracao.Name = "txtDuracao";
            this.txtDuracao.PlaceholderText = "Duração (min)";
            this.txtDuracao.Size = new System.Drawing.Size(190, 36);
            this.txtDuracao.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtDuracao.ForeColor = System.Drawing.Color.White;
            
            // txtDiretor
            this.txtDiretor.Location = new System.Drawing.Point(30, 230);
            this.txtDiretor.Name = "txtDiretor";
            this.txtDiretor.PlaceholderText = "Diretor";
            this.txtDiretor.Size = new System.Drawing.Size(400, 36);
            this.txtDiretor.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtDiretor.ForeColor = System.Drawing.Color.White;
            
            // txtElenco
            this.txtElenco.Location = new System.Drawing.Point(30, 280);
            this.txtElenco.Name = "txtElenco";
            this.txtElenco.PlaceholderText = "Elenco";
            this.txtElenco.Size = new System.Drawing.Size(400, 36);
            this.txtElenco.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtElenco.ForeColor = System.Drawing.Color.White;
            
            // txtCategoriaId
            this.txtCategoriaId.Location = new System.Drawing.Point(30, 330);
            this.txtCategoriaId.Name = "txtCategoriaId";
            this.txtCategoriaId.PlaceholderText = "ID da Categoria";
            this.txtCategoriaId.Size = new System.Drawing.Size(190, 36);
            this.txtCategoriaId.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtCategoriaId.ForeColor = System.Drawing.Color.White;
            
            // txtClassificacaoId
            this.txtClassificacaoId.Location = new System.Drawing.Point(240, 330);
            this.txtClassificacaoId.Name = "txtClassificacaoId";
            this.txtClassificacaoId.PlaceholderText = "ID da Classificação";
            this.txtClassificacaoId.Size = new System.Drawing.Size(190, 36);
            this.txtClassificacaoId.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtClassificacaoId.ForeColor = System.Drawing.Color.White;
            
            // chkAtivo
            this.chkAtivo.AutoSize = true;
            this.chkAtivo.Location = new System.Drawing.Point(30, 385);
            this.chkAtivo.Name = "chkAtivo";
            this.chkAtivo.Size = new System.Drawing.Size(54, 19);
            this.chkAtivo.Text = "Ativo";
            this.chkAtivo.ForeColor = System.Drawing.Color.White;
            
            // chkDestaqueHome
            this.chkDestaqueHome.AutoSize = true;
            this.chkDestaqueHome.Location = new System.Drawing.Point(120, 385);
            this.chkDestaqueHome.Name = "chkDestaqueHome";
            this.chkDestaqueHome.Size = new System.Drawing.Size(76, 19);
            this.chkDestaqueHome.Text = "Destaque";
            this.chkDestaqueHome.ForeColor = System.Drawing.Color.White;
            
            // LADO DIREITO
            
            // txtUrlCapa
            this.txtUrlCapa.Location = new System.Drawing.Point(460, 80);
            this.txtUrlCapa.Name = "txtUrlCapa";
            this.txtUrlCapa.PlaceholderText = "URL da Capa";
            this.txtUrlCapa.Size = new System.Drawing.Size(260, 36);
            this.txtUrlCapa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtUrlCapa.ForeColor = System.Drawing.Color.White;
            
            // btnUploadCapa
            this.btnUploadCapa.Location = new System.Drawing.Point(730, 80);
            this.btnUploadCapa.Name = "btnUploadCapa";
            this.btnUploadCapa.Size = new System.Drawing.Size(90, 36);
            this.btnUploadCapa.Text = "Upload";
            this.btnUploadCapa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            
            // picPreviewCapa
            this.picPreviewCapa.Location = new System.Drawing.Point(460, 130);
            this.picPreviewCapa.Name = "picPreviewCapa";
            this.picPreviewCapa.Size = new System.Drawing.Size(120, 180);
            this.picPreviewCapa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreviewCapa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            
            // picPreviewBanner
            this.picPreviewBanner.Location = new System.Drawing.Point(600, 130);
            this.picPreviewBanner.Name = "picPreviewBanner";
            this.picPreviewBanner.Size = new System.Drawing.Size(220, 180);
            this.picPreviewBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreviewBanner.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            
            // txtUrlBanner
            this.txtUrlBanner.Location = new System.Drawing.Point(460, 330);
            this.txtUrlBanner.Name = "txtUrlBanner";
            this.txtUrlBanner.PlaceholderText = "URL do Banner";
            this.txtUrlBanner.Size = new System.Drawing.Size(260, 36);
            this.txtUrlBanner.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtUrlBanner.ForeColor = System.Drawing.Color.White;
            
            // btnUploadBanner
            this.btnUploadBanner.Location = new System.Drawing.Point(730, 330);
            this.btnUploadBanner.Name = "btnUploadBanner";
            this.btnUploadBanner.Size = new System.Drawing.Size(90, 36);
            this.btnUploadBanner.Text = "Upload";
            this.btnUploadBanner.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            
            // txtTrailerUrl
            this.txtTrailerUrl.Location = new System.Drawing.Point(460, 380);
            this.txtTrailerUrl.Name = "txtTrailerUrl";
            this.txtTrailerUrl.PlaceholderText = "URL do Trailer (YouTube)";
            this.txtTrailerUrl.Size = new System.Drawing.Size(260, 36);
            this.txtTrailerUrl.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtTrailerUrl.ForeColor = System.Drawing.Color.White;
            
            // btnVisualizarTrailer
            this.btnVisualizarTrailer.Location = new System.Drawing.Point(730, 380);
            this.btnVisualizarTrailer.Name = "btnVisualizarTrailer";
            this.btnVisualizarTrailer.Size = new System.Drawing.Size(90, 36);
            this.btnVisualizarTrailer.Text = "Trailer";
            this.btnVisualizarTrailer.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            
            // txtVideoUrl
            this.txtVideoUrl.Location = new System.Drawing.Point(460, 430);
            this.txtVideoUrl.Name = "txtVideoUrl";
            this.txtVideoUrl.PlaceholderText = "URL do Filme";
            this.txtVideoUrl.Size = new System.Drawing.Size(260, 36);
            this.txtVideoUrl.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtVideoUrl.ForeColor = System.Drawing.Color.White;
            
            // btnVisualizarFilme
            this.btnVisualizarFilme.Location = new System.Drawing.Point(730, 430);
            this.btnVisualizarFilme.Name = "btnVisualizarFilme";
            this.btnVisualizarFilme.Size = new System.Drawing.Size(90, 36);
            this.btnVisualizarFilme.Text = "Filme";
            this.btnVisualizarFilme.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            
            // btnCancelar
            this.btnCancelar.Location = new System.Drawing.Point(560, 500);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 36);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            
            // btnSalvar
            this.btnSalvar.Location = new System.Drawing.Point(700, 500);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(120, 36);
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(107)))), ((int)(((byte)(0)))));
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            
            // FormFilme
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(14)))));
            this.ClientSize = new System.Drawing.Size(850, 560);
            this.Controls.Add(this.chkAtivo);
            this.Controls.Add(this.chkDestaqueHome);
            this.Controls.Add(this.txtUrlCapa);
            this.Controls.Add(this.btnUploadCapa);
            this.Controls.Add(this.picPreviewCapa);
            this.Controls.Add(this.txtUrlBanner);
            this.Controls.Add(this.btnUploadBanner);
            this.Controls.Add(this.picPreviewBanner);
            this.Controls.Add(this.txtTrailerUrl);
            this.Controls.Add(this.btnVisualizarTrailer);
            this.Controls.Add(this.txtVideoUrl);
            this.Controls.Add(this.btnVisualizarFilme);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.txtClassificacaoId);
            this.Controls.Add(this.txtCategoriaId);
            this.Controls.Add(this.txtElenco);
            this.Controls.Add(this.txtDiretor);
            this.Controls.Add(this.txtDuracao);
            this.Controls.Add(this.txtAno);
            this.Controls.Add(this.txtDescricao);
            this.Controls.Add(this.txtTitulo);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cadastro de Filme";
            this.Load += new System.EventHandler(this.FormFilme_Load);
            
            ((System.ComponentModel.ISupportInitialize)(this.picPreviewCapa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreviewBanner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtTitulo;
        private Guna.UI2.WinForms.Guna2TextBox txtDescricao;
        private Guna.UI2.WinForms.Guna2TextBox txtAno;
        private Guna.UI2.WinForms.Guna2TextBox txtDuracao;
        private Guna.UI2.WinForms.Guna2TextBox txtDiretor;
        private Guna.UI2.WinForms.Guna2TextBox txtElenco;
        private Guna.UI2.WinForms.Guna2TextBox txtCategoriaId;
        private Guna.UI2.WinForms.Guna2TextBox txtClassificacaoId;
        
        private Guna.UI2.WinForms.Guna2CheckBox chkAtivo;
        private Guna.UI2.WinForms.Guna2CheckBox chkDestaqueHome;
        
        private Guna.UI2.WinForms.Guna2TextBox txtUrlCapa;
        private Guna.UI2.WinForms.Guna2Button btnUploadCapa;
        private Guna.UI2.WinForms.Guna2PictureBox picPreviewCapa;
        
        private Guna.UI2.WinForms.Guna2TextBox txtUrlBanner;
        private Guna.UI2.WinForms.Guna2Button btnUploadBanner;
        private Guna.UI2.WinForms.Guna2PictureBox picPreviewBanner;
        
        private Guna.UI2.WinForms.Guna2TextBox txtTrailerUrl;
        private Guna.UI2.WinForms.Guna2Button btnVisualizarTrailer;
        
        private Guna.UI2.WinForms.Guna2TextBox txtVideoUrl;
        private Guna.UI2.WinForms.Guna2Button btnVisualizarFilme;

        private Guna.UI2.WinForms.Guna2Button btnSalvar;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
    }
}
