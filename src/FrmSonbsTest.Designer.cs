﻿namespace SonbsTest
{
    partial class FrmSonbsTest
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSonbsTest));
            btnSignInStart = new Button();
            btnSignInEnd = new Button();
            groupSignIn = new GroupBox();
            groupVotazione = new GroupBox();
            viewVoteResult = new ListView();
            colheadVoteName = new ColumnHeader();
            colheadVoteResult = new ColumnHeader();
            label2 = new Label();
            btnVotazioneEnd = new Button();
            btnVotazioneStart = new Button();
            cmbVotazioneTipi = new ComboBox();
            viewDelegates = new ListView();
            colheadName = new ColumnHeader();
            colheadSurname = new ColumnHeader();
            colheadGruppo = new ColumnHeader();
            colheadSeat = new ColumnHeader();
            colheadTalk = new ColumnHeader();
            viewOrdini = new ListView();
            colheadDescrizione = new ColumnHeader();
            btnSittingStart = new Button();
            btnSittingStop = new Button();
            btnConnectSonbs = new Button();
            btnConnectGaravot = new Button();
            btnSendTopic = new Button();
            btnSendTalkOn = new Button();
            btnSendTalkOff = new Button();
            btnCloseAllMics = new Button();
            tcDelegates = new TabControl();
            tabSonbs = new TabPage();
            viewSonbs = new ListView();
            colheadMicId = new ColumnHeader();
            colheadWired = new ColumnHeader();
            colheadChairman = new ColumnHeader();
            colheadAltro = new ColumnHeader();
            tabGaravot = new TabPage();
            btnConfirmTalkRequest = new Button();
            btnRefuteTalkRequest = new Button();
            ssLog = new StatusStrip();
            tslLog = new ToolStripStatusLabel();
            cmsOdg = new ContextMenuStrip(components);
            cmsMenuAddOdg = new ToolStripMenuItem();
            groupSignIn.SuspendLayout();
            groupVotazione.SuspendLayout();
            tcDelegates.SuspendLayout();
            tabSonbs.SuspendLayout();
            tabGaravot.SuspendLayout();
            ssLog.SuspendLayout();
            cmsOdg.SuspendLayout();
            SuspendLayout();
            // 
            // btnSignInStart
            // 
            btnSignInStart.Location = new Point(6, 22);
            btnSignInStart.Name = "btnSignInStart";
            btnSignInStart.Size = new Size(75, 23);
            btnSignInStart.TabIndex = 0;
            btnSignInStart.Text = "AVVIA";
            btnSignInStart.UseVisualStyleBackColor = true;
            btnSignInStart.Click += btnSignInStart_Click;
            // 
            // btnSignInEnd
            // 
            btnSignInEnd.Location = new Point(95, 22);
            btnSignInEnd.Name = "btnSignInEnd";
            btnSignInEnd.Size = new Size(67, 23);
            btnSignInEnd.TabIndex = 1;
            btnSignInEnd.Text = "FERMA";
            btnSignInEnd.UseVisualStyleBackColor = true;
            btnSignInEnd.Click += btnSignInEnd_Click;
            // 
            // groupSignIn
            // 
            groupSignIn.Controls.Add(btnSignInStart);
            groupSignIn.Controls.Add(btnSignInEnd);
            groupSignIn.Location = new Point(12, 12);
            groupSignIn.Name = "groupSignIn";
            groupSignIn.Size = new Size(200, 57);
            groupSignIn.TabIndex = 2;
            groupSignIn.TabStop = false;
            groupSignIn.Text = "SIGN IN";
            // 
            // groupVotazione
            // 
            groupVotazione.Controls.Add(viewVoteResult);
            groupVotazione.Controls.Add(label2);
            groupVotazione.Controls.Add(btnVotazioneEnd);
            groupVotazione.Controls.Add(btnVotazioneStart);
            groupVotazione.Controls.Add(cmbVotazioneTipi);
            groupVotazione.Location = new Point(12, 75);
            groupVotazione.Name = "groupVotazione";
            groupVotazione.Size = new Size(200, 363);
            groupVotazione.TabIndex = 3;
            groupVotazione.TabStop = false;
            groupVotazione.Text = "Votazione";
            // 
            // viewVoteResult
            // 
            viewVoteResult.Columns.AddRange(new ColumnHeader[] { colheadVoteName, colheadVoteResult });
            viewVoteResult.GridLines = true;
            viewVoteResult.Location = new Point(6, 104);
            viewVoteResult.MultiSelect = false;
            viewVoteResult.Name = "viewVoteResult";
            viewVoteResult.Scrollable = false;
            viewVoteResult.Size = new Size(188, 253);
            viewVoteResult.TabIndex = 6;
            viewVoteResult.UseCompatibleStateImageBehavior = false;
            viewVoteResult.View = View.Details;
            // 
            // colheadVoteName
            // 
            colheadVoteName.Text = "Nome";
            colheadVoteName.Width = 120;
            // 
            // colheadVoteResult
            // 
            colheadVoteResult.Text = "#";
            colheadVoteResult.Width = 25;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 86);
            label2.Name = "label2";
            label2.Size = new Size(49, 15);
            label2.TabIndex = 5;
            label2.Text = "Risultati";
            // 
            // btnVotazioneEnd
            // 
            btnVotazioneEnd.Location = new Point(87, 51);
            btnVotazioneEnd.Name = "btnVotazioneEnd";
            btnVotazioneEnd.Size = new Size(75, 23);
            btnVotazioneEnd.TabIndex = 3;
            btnVotazioneEnd.Text = "CONCLUDI";
            btnVotazioneEnd.UseVisualStyleBackColor = true;
            btnVotazioneEnd.Click += btnVotazioneEnd_Click;
            // 
            // btnVotazioneStart
            // 
            btnVotazioneStart.Location = new Point(6, 51);
            btnVotazioneStart.Name = "btnVotazioneStart";
            btnVotazioneStart.Size = new Size(75, 23);
            btnVotazioneStart.TabIndex = 2;
            btnVotazioneStart.Text = "AVVIA";
            btnVotazioneStart.UseVisualStyleBackColor = true;
            btnVotazioneStart.Click += btnVotazioneStart_Click;
            // 
            // cmbVotazioneTipi
            // 
            cmbVotazioneTipi.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVotazioneTipi.Items.AddRange(new object[] { "LastValidDirectVoting", "LastValidVotingAfterSigningIn", "FirstValidDirectVoting", "FirstValidVotingAfterSigningIn", "LastValidDirectElection", "LastValidElectionAfterSigningIn", "FirstValidDirectElection", "FirstValidElectionAfterSigningIn", "FirstValidDirectRating", "FirstValidRatingAfterSigningIn", "LastValidDirectRating", "LastValidRatingAfterSigningIn", "FirstValidDirectTwoItems", "FirstValidTwoItemsAfterSigningIn", "LastValidThreeItemsAfterSigningIn", "LastValidThreeItemsAfterSigningIn_2", "LastValidThreeItemsAfterSigningIn_3", "LastValidThreeItemsAfterSigningIn_4", "LastValidDirectThreeItems", "FirstValidDirectThreeItems", "FirstValidThreeItemsAfterSigningIn", "LastValidThreeItemsAfterSigningIn_5", "LastValidDirectFourItems", "FirstValidDirectFourItems", "FirstValidFourItemsAfterSigningIn", "LastValidFourItemsAfterSigningIn", "LastValidDirectFourItems_2", "FirstValidDirectFourItems_2", "Unknown1", "Unknown2", "Unknown3", "Unknown4" });
            cmbVotazioneTipi.Location = new Point(6, 22);
            cmbVotazioneTipi.Name = "cmbVotazioneTipi";
            cmbVotazioneTipi.Size = new Size(188, 23);
            cmbVotazioneTipi.TabIndex = 0;
            // 
            // viewDelegates
            // 
            viewDelegates.Columns.AddRange(new ColumnHeader[] { colheadName, colheadSurname, colheadGruppo, colheadSeat, colheadTalk });
            viewDelegates.Dock = DockStyle.Fill;
            viewDelegates.FullRowSelect = true;
            viewDelegates.Location = new Point(3, 3);
            viewDelegates.MultiSelect = false;
            viewDelegates.Name = "viewDelegates";
            viewDelegates.Size = new Size(421, 194);
            viewDelegates.TabIndex = 4;
            viewDelegates.UseCompatibleStateImageBehavior = false;
            viewDelegates.View = View.Details;
            // 
            // colheadName
            // 
            colheadName.Text = "Nome";
            colheadName.Width = 100;
            // 
            // colheadSurname
            // 
            colheadSurname.Text = "Cognome";
            colheadSurname.Width = 100;
            // 
            // colheadGruppo
            // 
            colheadGruppo.Text = "Gruppo";
            colheadGruppo.Width = 120;
            // 
            // colheadSeat
            // 
            colheadSeat.Text = "Seat";
            colheadSeat.Width = 40;
            // 
            // colheadTalk
            // 
            colheadTalk.Text = "🔊";
            colheadTalk.Width = 30;
            // 
            // viewOrdini
            // 
            viewOrdini.Columns.AddRange(new ColumnHeader[] { colheadDescrizione });
            viewOrdini.ContextMenuStrip = cmsOdg;
            viewOrdini.FullRowSelect = true;
            viewOrdini.GridLines = true;
            viewOrdini.Location = new Point(218, 255);
            viewOrdini.MultiSelect = false;
            viewOrdini.Name = "viewOrdini";
            viewOrdini.Size = new Size(477, 183);
            viewOrdini.TabIndex = 5;
            viewOrdini.UseCompatibleStateImageBehavior = false;
            viewOrdini.View = View.Details;
            viewOrdini.SelectedIndexChanged += viewOrdini_SelectedIndexChanged;
            viewOrdini.DoubleClick += viewOrdini_DoubleClick;
            // 
            // colheadDescrizione
            // 
            colheadDescrizione.Text = "Ordine del giorno";
            colheadDescrizione.Width = 300;
            // 
            // btnSittingStart
            // 
            btnSittingStart.Location = new Point(18, 444);
            btnSittingStart.Name = "btnSittingStart";
            btnSittingStart.Size = new Size(92, 23);
            btnSittingStart.TabIndex = 6;
            btnSittingStart.Text = "Apri Seduta";
            btnSittingStart.UseVisualStyleBackColor = true;
            btnSittingStart.Click += btnSittingStart_Click;
            // 
            // btnSittingStop
            // 
            btnSittingStop.Location = new Point(116, 444);
            btnSittingStop.Name = "btnSittingStop";
            btnSittingStop.Size = new Size(102, 23);
            btnSittingStop.TabIndex = 7;
            btnSittingStop.Text = "Chiudi Seduta";
            btnSittingStop.UseVisualStyleBackColor = true;
            btnSittingStop.Click += btnSittingStop_Click;
            // 
            // btnConnectSonbs
            // 
            btnConnectSonbs.Location = new Point(224, 444);
            btnConnectSonbs.Name = "btnConnectSonbs";
            btnConnectSonbs.Size = new Size(127, 23);
            btnConnectSonbs.TabIndex = 8;
            btnConnectSonbs.Text = "Connetti SONBS";
            btnConnectSonbs.UseVisualStyleBackColor = true;
            btnConnectSonbs.Click += btnConnectSonbs_Click;
            // 
            // btnConnectGaravot
            // 
            btnConnectGaravot.Location = new Point(357, 444);
            btnConnectGaravot.Name = "btnConnectGaravot";
            btnConnectGaravot.Size = new Size(127, 23);
            btnConnectGaravot.TabIndex = 9;
            btnConnectGaravot.Text = "Auth Garavot";
            btnConnectGaravot.UseVisualStyleBackColor = true;
            btnConnectGaravot.Click += btnConnectGaravot_Click;
            // 
            // btnSendTopic
            // 
            btnSendTopic.Enabled = false;
            btnSendTopic.Location = new Point(592, 444);
            btnSendTopic.Name = "btnSendTopic";
            btnSendTopic.Size = new Size(103, 23);
            btnSendTopic.TabIndex = 10;
            btnSendTopic.Text = "Invia ODG";
            btnSendTopic.UseVisualStyleBackColor = true;
            btnSendTopic.Click += btnSendTopic_Click;
            // 
            // btnSendTalkOn
            // 
            btnSendTalkOn.Enabled = false;
            btnSendTalkOn.Location = new Point(218, 226);
            btnSendTalkOn.Name = "btnSendTalkOn";
            btnSendTalkOn.Size = new Size(79, 23);
            btnSendTalkOn.TabIndex = 11;
            btnSendTalkOn.Text = "Apri parola";
            btnSendTalkOn.UseVisualStyleBackColor = true;
            btnSendTalkOn.Click += btnSendTalkOn_Click;
            // 
            // btnSendTalkOff
            // 
            btnSendTalkOff.Enabled = false;
            btnSendTalkOff.Location = new Point(303, 226);
            btnSendTalkOff.Name = "btnSendTalkOff";
            btnSendTalkOff.Size = new Size(87, 23);
            btnSendTalkOff.TabIndex = 12;
            btnSendTalkOff.Text = "Chiudi parola";
            btnSendTalkOff.UseVisualStyleBackColor = true;
            btnSendTalkOff.Click += btnSendTalkOff_Click;
            // 
            // btnCloseAllMics
            // 
            btnCloseAllMics.Location = new Point(396, 226);
            btnCloseAllMics.Name = "btnCloseAllMics";
            btnCloseAllMics.Size = new Size(108, 23);
            btnCloseAllMics.TabIndex = 13;
            btnCloseAllMics.Text = "Chiudi microfoni";
            btnCloseAllMics.UseVisualStyleBackColor = true;
            btnCloseAllMics.Click += btnCloseAllMics_Click;
            // 
            // tcDelegates
            // 
            tcDelegates.Alignment = TabAlignment.Left;
            tcDelegates.Controls.Add(tabSonbs);
            tcDelegates.Controls.Add(tabGaravot);
            tcDelegates.ItemSize = new Size(26, 48);
            tcDelegates.Location = new Point(212, 12);
            tcDelegates.Multiline = true;
            tcDelegates.Name = "tcDelegates";
            tcDelegates.SelectedIndex = 0;
            tcDelegates.Size = new Size(483, 208);
            tcDelegates.TabIndex = 14;
            // 
            // tabSonbs
            // 
            tabSonbs.Controls.Add(viewSonbs);
            tabSonbs.Location = new Point(52, 4);
            tabSonbs.Name = "tabSonbs";
            tabSonbs.Padding = new Padding(3);
            tabSonbs.Size = new Size(427, 200);
            tabSonbs.TabIndex = 0;
            tabSonbs.Text = "Sonbs";
            tabSonbs.UseVisualStyleBackColor = true;
            // 
            // viewSonbs
            // 
            viewSonbs.Columns.AddRange(new ColumnHeader[] { colheadMicId, colheadWired, colheadChairman, colheadAltro });
            viewSonbs.Dock = DockStyle.Fill;
            viewSonbs.FullRowSelect = true;
            viewSonbs.Location = new Point(3, 3);
            viewSonbs.MultiSelect = false;
            viewSonbs.Name = "viewSonbs";
            viewSonbs.Size = new Size(421, 194);
            viewSonbs.TabIndex = 5;
            viewSonbs.UseCompatibleStateImageBehavior = false;
            viewSonbs.View = View.Details;
            // 
            // colheadMicId
            // 
            colheadMicId.Text = "Id";
            colheadMicId.Width = 25;
            // 
            // colheadWired
            // 
            colheadWired.Text = "Wired";
            // 
            // colheadChairman
            // 
            colheadChairman.Text = "Chairman";
            colheadChairman.Width = 65;
            // 
            // colheadAltro
            // 
            colheadAltro.Text = "Stato";
            colheadAltro.Width = 90;
            // 
            // tabGaravot
            // 
            tabGaravot.Controls.Add(viewDelegates);
            tabGaravot.Location = new Point(52, 4);
            tabGaravot.Name = "tabGaravot";
            tabGaravot.Padding = new Padding(3);
            tabGaravot.Size = new Size(427, 200);
            tabGaravot.TabIndex = 1;
            tabGaravot.Text = "Garavot";
            tabGaravot.UseVisualStyleBackColor = true;
            // 
            // btnConfirmTalkRequest
            // 
            btnConfirmTalkRequest.Enabled = false;
            btnConfirmTalkRequest.Location = new Point(510, 226);
            btnConfirmTalkRequest.Name = "btnConfirmTalkRequest";
            btnConfirmTalkRequest.Size = new Size(63, 23);
            btnConfirmTalkRequest.TabIndex = 15;
            btnConfirmTalkRequest.Text = "Accetta";
            btnConfirmTalkRequest.UseVisualStyleBackColor = true;
            btnConfirmTalkRequest.Click += btnConfirmTalkRequest_Click;
            // 
            // btnRefuteTalkRequest
            // 
            btnRefuteTalkRequest.Enabled = false;
            btnRefuteTalkRequest.Location = new Point(579, 226);
            btnRefuteTalkRequest.Name = "btnRefuteTalkRequest";
            btnRefuteTalkRequest.Size = new Size(57, 23);
            btnRefuteTalkRequest.TabIndex = 16;
            btnRefuteTalkRequest.Text = "Nega";
            btnRefuteTalkRequest.UseVisualStyleBackColor = true;
            btnRefuteTalkRequest.Click += btnRefuteTalkRequest_Click;
            // 
            // ssLog
            // 
            ssLog.Items.AddRange(new ToolStripItem[] { tslLog });
            ssLog.Location = new Point(0, 471);
            ssLog.Name = "ssLog";
            ssLog.Size = new Size(700, 22);
            ssLog.SizingGrip = false;
            ssLog.TabIndex = 17;
            // 
            // tslLog
            // 
            tslLog.Name = "tslLog";
            tslLog.Size = new Size(0, 17);
            // 
            // cmsOdg
            // 
            cmsOdg.Items.AddRange(new ToolStripItem[] { cmsMenuAddOdg });
            cmsOdg.Name = "cmsOdg";
            cmsOdg.Size = new Size(150, 26);
            // 
            // cmsMenuAddOdg
            // 
            cmsMenuAddOdg.Name = "cmsMenuAddOdg";
            cmsMenuAddOdg.Size = new Size(149, 22);
            cmsMenuAddOdg.Text = "Aggiungi Odg";
            cmsMenuAddOdg.Click += cmsMenuAddOdg_Click;
            // 
            // FrmSonbsTest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 493);
            Controls.Add(ssLog);
            Controls.Add(btnRefuteTalkRequest);
            Controls.Add(btnConfirmTalkRequest);
            Controls.Add(tcDelegates);
            Controls.Add(btnCloseAllMics);
            Controls.Add(btnSendTalkOff);
            Controls.Add(btnSendTalkOn);
            Controls.Add(btnSendTopic);
            Controls.Add(btnConnectGaravot);
            Controls.Add(btnConnectSonbs);
            Controls.Add(btnSittingStop);
            Controls.Add(btnSittingStart);
            Controls.Add(viewOrdini);
            Controls.Add(groupVotazione);
            Controls.Add(groupSignIn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSonbsTest";
            Text = "Test Seduta Sonbs";
            FormClosing += FrmSonbsTest_FormClosingAsync;
            groupSignIn.ResumeLayout(false);
            groupVotazione.ResumeLayout(false);
            groupVotazione.PerformLayout();
            tcDelegates.ResumeLayout(false);
            tabSonbs.ResumeLayout(false);
            tabGaravot.ResumeLayout(false);
            ssLog.ResumeLayout(false);
            ssLog.PerformLayout();
            cmsOdg.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSignInStart;
        private Button btnSignInEnd;
        private GroupBox groupSignIn;
        private GroupBox groupVotazione;
        private Label label2;
        private Button btnVotazioneEnd;
        private Button btnVotazioneStart;
        private ComboBox cmbVotazioneTipi;
        private ListView viewVoteResult;
        private ColumnHeader colheadVoteName;
        private ColumnHeader colheadVoteResult;
        private ListView viewDelegates;
        private ListView viewOrdini;
        private ColumnHeader colheadName;
        private ColumnHeader colheadSurname;
        private ColumnHeader colheadGruppo;
        private ColumnHeader colheadDescrizione;
        private Button btnSittingStart;
        private Button btnSittingStop;
        private Button btnConnectSonbs;
        private ColumnHeader colheadSeat;
        private Button btnConnectGaravot;
        private Button btnSendTopic;
        private Button btnSendTalkOn;
        private ColumnHeader colheadTalk;
        private Button btnSendTalkOff;
        private Button btnCloseAllMics;
        private TabControl tcDelegates;
        private TabPage tabSonbs;
        private TabPage tabGaravot;
        private ListView viewSonbs;
        private ColumnHeader colheadMicId;
        private ColumnHeader colheadWired;
        private ColumnHeader colheadChairman;
        private ColumnHeader colheadAltro;
        private Button btnConfirmTalkRequest;
        private Button btnRefuteTalkRequest;
        private StatusStrip ssLog;
        private ToolStripStatusLabel tslLog;
        private ContextMenuStrip cmsOdg;
        private ToolStripMenuItem cmsMenuAddOdg;
    }
}
