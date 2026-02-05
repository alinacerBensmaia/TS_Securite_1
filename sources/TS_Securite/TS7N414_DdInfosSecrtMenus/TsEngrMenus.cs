using System.Collections.Generic;
using System;
using Rrq.InfrastructureLotPFI.GestionFichier;
using System.Linq;


namespace TS7N414_DdInfosSecrtMenus
{
	public class TsEngrMenus : XdIEnregistrement
	{

		private static XdCuStruct mRacine;

		public string CoEnv { get; set; }
		public string NomTsConsrPanrm { get; set; }
		public string CoNivAccSec { get; set; }
		public string CodActnEcran { get; set; }
		public string NumChoix { get; set; }
		public string DesChoixMenu { get; set; }
		public string DesTitreAffch { get; set; }
		public string CodIndctApplc { get; set; }

		static TsEngrMenus()
		{
			mRacine = new XdCuStruct("Root", "TsEngrMenus", AssignermRacine, (Valeur, y) => new string[1]{string.Empty}, false, false);

			var Elem1 = new XdCuStruct("CoEnv", "ValeurString", 4, AssignerElem1, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrMenus) y).CoEnv), false, false, false, false);
			mRacine.AjouterSousStructure(Elem1);
			Elem1.PadCaractere = ' ';
			Elem1.PadADroite = true;

			var Elem2 = new XdCuStruct("NomTsConsrPanrm", "ValeurString", 5, AssignerElem2, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrMenus) y).NomTsConsrPanrm), false, false, false, false);
			mRacine.AjouterSousStructure(Elem2);
			Elem2.PadCaractere = ' ';
			Elem2.PadADroite = true;

			var Elem3 = new XdCuStruct("CoNivAccSec", "ValeurString", 2, AssignerElem3, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrMenus) y).CoNivAccSec), false, false, false, false);
			mRacine.AjouterSousStructure(Elem3);
			Elem3.PadCaractere = ' ';
			Elem3.PadADroite = true;

			var Elem4 = new XdCuStruct("CodActnEcran", "ValeurString", 3, AssignerElem4, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrMenus) y).CodActnEcran), false, false, false, false);
			mRacine.AjouterSousStructure(Elem4);
			Elem4.PadCaractere = ' ';
			Elem4.PadADroite = true;

			var Elem5 = new XdCuStruct("NumChoix", "ValeurString", 7, AssignerElem5, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrMenus) y).NumChoix), false, false, false, false);
			mRacine.AjouterSousStructure(Elem5);
			Elem5.PadCaractere = ' ';
			Elem5.PadADroite = true;

			var Elem6 = new XdCuStruct("DesChoixMenu", "ValeurString", 103, AssignerElem6, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrMenus) y).DesChoixMenu), false, false, false, false);
			mRacine.AjouterSousStructure(Elem6);
			Elem6.PadCaractere = ' ';
			Elem6.PadADroite = true;

			var Elem7 = new XdCuStruct("DesTitreAffch", "ValeurString", 1, AssignerElem7, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrMenus) y).DesTitreAffch), false, false, false, false);
			mRacine.AjouterSousStructure(Elem7);
			Elem7.PadCaractere = ' ';
			Elem7.PadADroite = true;

			var Elem8 = new XdCuStruct("CodIndctApplc", "ValeurString", 1, AssignerElem8, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrMenus) y).CodIndctApplc), false, false, false, false);
			mRacine.AjouterSousStructure(Elem8);
			Elem8.PadCaractere = ' ';
			Elem8.PadADroite = true;
		}

		private static void AssignermRacine(XdCuValeurEnreg Valeur,XdIEnregistrement Enregistrement)
		{
		}
		private static void AssignerElem1(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrMenus) Enregistrement).CoEnv = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem2(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrMenus) Enregistrement).NomTsConsrPanrm = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem3(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrMenus) Enregistrement).CoNivAccSec = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem4(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrMenus) Enregistrement).CodActnEcran = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem5(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrMenus) Enregistrement).NumChoix = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem6(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrMenus) Enregistrement).DesChoixMenu = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem7(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrMenus) Enregistrement).DesTitreAffch = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem8(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrMenus) Enregistrement).CodIndctApplc = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}

		public void AssignerDonnees(XdCuStruct Donnees)
		{
			Donnees.RemplirEnregistrement(this);
		}
		public void ObtenirDonnees(XdCuStruct Donnees)
		{
			Donnees.ViderEnregistrement(this);
		}
		public XdCuStruct ObtenirStructure()
		{
			return mRacine;
		}


		public TsEngrMenus()
		{
			CoEnv = string.Empty;
			NomTsConsrPanrm = string.Empty;
			CoNivAccSec = string.Empty;
			CodActnEcran = string.Empty;
			NumChoix = string.Empty;
			DesChoixMenu = string.Empty;
			DesTitreAffch = string.Empty;
			CodIndctApplc = string.Empty;
		}

	}
}
