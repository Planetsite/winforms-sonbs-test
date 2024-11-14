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
            label1 = new Label();
            cmbVotazioneTipi = new ComboBox();
            viewDelegates = new ListView();
            colheadName = new ColumnHeader();
            colheadSurname = new ColumnHeader();
            colheadGruppo = new ColumnHeader();
            colheadSeat = new ColumnHeader();
            colheadTalk = new ColumnHeader();
            viewOrdini = new ListView();
            colheadDescrizione = new ColumnHeader();
            cmdSittingStart = new Button();
            cmdSittingStop = new Button();
            cmdConnectSonbs = new Button();
            btnConnectGaravot = new Button();
            cmdSendTopic = new Button();
            cmdSendTalkOn = new Button();
            cmdSendTalkOff = new Button();
            cmdCloseAllMics = new Button();
            tcXXX = new TabControl();
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
            groupSignIn.SuspendLayout();
            groupVotazione.SuspendLayout();
            tcXXX.SuspendLayout();
            tabSonbs.SuspendLayout();
            tabGaravot.SuspendLayout();
            ssLog.SuspendLayout();
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
            // 
            // btnSignInEnd
            // 
            btnSignInEnd.Location = new Point(95, 22);
            btnSignInEnd.Name = "btnSignInEnd";
            btnSignInEnd.Size = new Size(67, 23);
            btnSignInEnd.TabIndex = 1;
            btnSignInEnd.Text = "FERMA";
            btnSignInEnd.UseVisualStyleBackColor = true;
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
            groupVotazione.Controls.Add(label1);
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(30, 15);
            label1.TabIndex = 1;
            label1.Text = "Tipo";
            // 
            // cmbVotazioneTipi
            // 
            cmbVotazioneTipi.FormattingEnabled = true;
            cmbVotazioneTipi.Items.AddRange(new object[] { "LastValidDirectVoting", "LastValidVotingAfterSigningIn", "FirstValidDirectVoting", "FirstValidVotingAfterSigningIn", "LastValidDirectElection", "LastValidElectionAfterSigningIn", "FirstValidDirectElection", "FirstValidElectionAfterSigningIn", "FirstValidDirectRating", "FirstValidRatingAfterSigningIn", "LastValidDirectRating", "LastValidRatingAfterSigningIn", "FirstValidDirectTwoItems", "FirstValidTwoItemsAfterSigningIn", "LastValidThreeItemsAfterSigningIn", "LastValidThreeItemsAfterSigningIn_2", "LastValidThreeItemsAfterSigningIn_3", "LastValidThreeItemsAfterSigningIn_4", "LastValidDirectThreeItems", "FirstValidDirectThreeItems", "FirstValidThreeItemsAfterSigningIn", "LastValidThreeItemsAfterSigningIn_5", "LastValidDirectFourItems", "FirstValidDirectFourItems", "FirstValidFourItemsAfterSigningIn", "LastValidFourItemsAfterSigningIn", "LastValidDirectFourItems_2", "FirstValidDirectFourItems_2", "Unknown1", "Unknown2", "Unknown3", "Unknown4" });
            cmbVotazioneTipi.Location = new Point(42, 22);
            cmbVotazioneTipi.Name = "cmbVotazioneTipi";
            cmbVotazioneTipi.Size = new Size(152, 23);
            cmbVotazioneTipi.TabIndex = 0;
            // 
            // viewDelegates
            // 
            viewDelegates.Columns.AddRange(new ColumnHeader[] { colheadName, colheadSurname, colheadGruppo, colheadSeat, colheadTalk });
            viewDelegates.Dock = DockStyle.Fill;
            viewDelegates.Location = new Point(3, 3);
            viewDelegates.Name = "viewDelegates";
            viewDelegates.Size = new Size(457, 171);
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
            colheadSeat.Text = "Mic#";
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
            viewOrdini.Location = new Point(218, 255);
            viewOrdini.Name = "viewOrdini";
            viewOrdini.Size = new Size(477, 183);
            viewOrdini.TabIndex = 5;
            viewOrdini.UseCompatibleStateImageBehavior = false;
            viewOrdini.View = View.Details;
            // 
            // colheadDescrizione
            // 
            colheadDescrizione.Text = "Ordine del giorno";
            colheadDescrizione.Width = 300;
            // 
            // cmdSittingStart
            // 
            cmdSittingStart.Location = new Point(18, 444);
            cmdSittingStart.Name = "cmdSittingStart";
            cmdSittingStart.Size = new Size(92, 23);
            cmdSittingStart.TabIndex = 6;
            cmdSittingStart.Text = "Apri Seduta";
            cmdSittingStart.UseVisualStyleBackColor = true;
            // 
            // cmdSittingStop
            // 
            cmdSittingStop.Location = new Point(116, 444);
            cmdSittingStop.Name = "cmdSittingStop";
            cmdSittingStop.Size = new Size(102, 23);
            cmdSittingStop.TabIndex = 7;
            cmdSittingStop.Text = "Chiudi Seduta";
            cmdSittingStop.UseVisualStyleBackColor = true;
            // 
            // cmdConnectSonbs
            // 
            cmdConnectSonbs.Location = new Point(224, 444);
            cmdConnectSonbs.Name = "cmdConnectSonbs";
            cmdConnectSonbs.Size = new Size(127, 23);
            cmdConnectSonbs.TabIndex = 8;
            cmdConnectSonbs.Text = "Connetti SONBS";
            cmdConnectSonbs.UseVisualStyleBackColor = true;
            cmdConnectSonbs.Click += cmdConnectSonbs_Click;
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
            // cmdSendTopic
            // 
            cmdSendTopic.Enabled = false;
            cmdSendTopic.Location = new Point(592, 444);
            cmdSendTopic.Name = "cmdSendTopic";
            cmdSendTopic.Size = new Size(103, 23);
            cmdSendTopic.TabIndex = 10;
            cmdSendTopic.Text = "Invia ODG";
            cmdSendTopic.UseVisualStyleBackColor = true;
            // 
            // cmdSendTalkOn
            // 
            cmdSendTalkOn.Enabled = false;
            cmdSendTalkOn.Location = new Point(218, 226);
            cmdSendTalkOn.Name = "cmdSendTalkOn";
            cmdSendTalkOn.Size = new Size(79, 23);
            cmdSendTalkOn.TabIndex = 11;
            cmdSendTalkOn.Text = "Apri parola";
            cmdSendTalkOn.UseVisualStyleBackColor = true;
            // 
            // cmdSendTalkOff
            // 
            cmdSendTalkOff.Enabled = false;
            cmdSendTalkOff.Location = new Point(303, 226);
            cmdSendTalkOff.Name = "cmdSendTalkOff";
            cmdSendTalkOff.Size = new Size(87, 23);
            cmdSendTalkOff.TabIndex = 12;
            cmdSendTalkOff.Text = "Chiudi parola";
            cmdSendTalkOff.UseVisualStyleBackColor = true;
            // 
            // cmdCloseAllMics
            // 
            cmdCloseAllMics.Location = new Point(396, 226);
            cmdCloseAllMics.Name = "cmdCloseAllMics";
            cmdCloseAllMics.Size = new Size(108, 23);
            cmdCloseAllMics.TabIndex = 13;
            cmdCloseAllMics.Text = "Chiudi microfoni";
            cmdCloseAllMics.UseVisualStyleBackColor = true;
            // 
            // tcXXX
            // 
            tcXXX.Appearance = TabAppearance.FlatButtons;
            tcXXX.Controls.Add(tabSonbs);
            tcXXX.Controls.Add(tabGaravot);
            tcXXX.Location = new Point(224, 12);
            tcXXX.Name = "tcXXX";
            tcXXX.SelectedIndex = 0;
            tcXXX.Size = new Size(471, 208);
            tcXXX.TabIndex = 14;
            // 
            // tabSonbs
            // 
            tabSonbs.Controls.Add(viewSonbs);
            tabSonbs.Location = new Point(4, 27);
            tabSonbs.Name = "tabSonbs";
            tabSonbs.Padding = new Padding(3);
            tabSonbs.Size = new Size(463, 177);
            tabSonbs.TabIndex = 0;
            tabSonbs.Text = "Sonbs";
            tabSonbs.UseVisualStyleBackColor = true;
            // 
            // viewSonbs
            // 
            viewSonbs.Columns.AddRange(new ColumnHeader[] { colheadMicId, colheadWired, colheadChairman, colheadAltro });
            viewSonbs.Dock = DockStyle.Fill;
            viewSonbs.Location = new Point(3, 3);
            viewSonbs.Name = "viewSonbs";
            viewSonbs.Size = new Size(457, 171);
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
            colheadAltro.Text = "Altro";
            colheadAltro.Width = 90;
            // 
            // tabGaravot
            // 
            tabGaravot.Controls.Add(viewDelegates);
            tabGaravot.Location = new Point(4, 27);
            tabGaravot.Name = "tabGaravot";
            tabGaravot.Padding = new Padding(3);
            tabGaravot.Size = new Size(463, 177);
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
            // 
            // btnRefuteTalkRequest
            // 
            btnRefuteTalkRequest.Enabled = false;
            btnRefuteTalkRequest.Location = new Point(579, 226);
            btnRefuteTalkRequest.Name = "btnRefuteTalkRequest";
            btnRefuteTalkRequest.Size = new Size(79, 23);
            btnRefuteTalkRequest.TabIndex = 16;
            btnRefuteTalkRequest.Text = "Rifiuta";
            btnRefuteTalkRequest.UseVisualStyleBackColor = true;
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
            // FrmSonbsTest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 493);
            Controls.Add(ssLog);
            Controls.Add(btnRefuteTalkRequest);
            Controls.Add(btnConfirmTalkRequest);
            Controls.Add(tcXXX);
            Controls.Add(cmdCloseAllMics);
            Controls.Add(cmdSendTalkOff);
            Controls.Add(cmdSendTalkOn);
            Controls.Add(cmdSendTopic);
            Controls.Add(btnConnectGaravot);
            Controls.Add(cmdConnectSonbs);
            Controls.Add(cmdSittingStop);
            Controls.Add(cmdSittingStart);
            Controls.Add(viewOrdini);
            Controls.Add(groupVotazione);
            Controls.Add(groupSignIn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSonbsTest";
            Text = "Test Seduta Sonbs";
            groupSignIn.ResumeLayout(false);
            groupVotazione.ResumeLayout(false);
            groupVotazione.PerformLayout();
            tcXXX.ResumeLayout(false);
            tabSonbs.ResumeLayout(false);
            tabGaravot.ResumeLayout(false);
            ssLog.ResumeLayout(false);
            ssLog.PerformLayout();
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
        private Label label1;
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
        private Button cmdSittingStart;
        private Button cmdSittingStop;
        private Button cmdConnectSonbs;
        private ColumnHeader colheadSeat;
        private Button btnConnectGaravot;
        private Button cmdSendTopic;
        private Button cmdSendTalkOn;
        private ColumnHeader colheadTalk;
        private Button cmdSendTalkOff;
        private Button cmdCloseAllMics;
        private TabControl tcXXX;
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
    }
}