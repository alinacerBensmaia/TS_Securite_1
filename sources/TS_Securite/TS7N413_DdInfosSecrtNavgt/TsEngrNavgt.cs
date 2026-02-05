using System.Collections.Generic;
using System;
using Rrq.InfrastructureLotPFI.GestionFichier;
using System.Linq;


namespace TS7N413_DdInfosSecrtNavgt
{
	public class TsEngrNavgt : XdIEnregistrement
	{

		private static XdCuStruct mRacine;

		public string CoEnv { get; set; }
		public string CoModSecNav { get; set; }
		public string CoProAccSec { get; set; }
		public string CoFonSec { get; set; }
		public string NmFonSec { get; set; }
		public string CoNivAccSec { get; set; }

		static TsEngrNavgt()
		{
			mRacine = new XdCuStruct("Root", "TsEngrNavgt", AssignermRacine, (Valeur, y) => new string[1]{string.Empty}, false, false);

			var Elem1 = new XdCuStruct("CoEnv", "ValeurString", 2, AssignerElem1, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrNavgt) y).CoEnv), false, false, false, false);
			mRacine.AjouterSousStructure(Elem1);
			Elem1.PadCaractere = ' ';
			Elem1.PadADroite = true;

			var Elem2 = new XdCuStruct("CoModSecNav", "ValeurString", 10, AssignerElem2, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrNavgt) y).CoModSecNav), false, false, false, false);
			mRacine.AjouterSousStructure(Elem2);
			Elem2.PadCaractere = ' ';
			Elem2.PadADroite = true;

			var Elem3 = new XdCuStruct("CoProAccSec", "ValeurString", 7, AssignerElem3, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrNavgt) y).CoProAccSec), false, false, false, false);
			mRacine.AjouterSousStructure(Elem3);
			Elem3.PadCaractere = ' ';
			Elem3.PadADroite = true;

			var Elem4 = new XdCuStruct("CoFonSec", "ValeurString", 10, AssignerElem4, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrNavgt) y).CoFonSec), false, false, false, false);
			mRacine.AjouterSousStructure(Elem4);
			Elem4.PadCaractere = ' ';
			Elem4.PadADroite = true;

			var Elem5 = new XdCuStruct("NmFonSec", "ValeurString", 53, AssignerElem5, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrNavgt) y).NmFonSec), false, false, false, false);
			mRacine.AjouterSousStructure(Elem5);
			Elem5.PadCaractere = ' ';
			Elem5.PadADroite = true;

			var Elem6 = new XdCuStruct("CoNivAccSec", "ValeurString", 4, AssignerElem6, (Valeur, y) => XdBaUtilitaireAccesFichier.ObtenirValeurElementDD(((TsEngrNavgt) y).CoNivAccSec), false, false, false, false);
			mRacine.AjouterSousStructure(Elem6);
			Elem6.PadCaractere = ' ';
			Elem6.PadADroite = true;
		}

		private static void AssignermRacine(XdCuValeurEnreg Valeur,XdIEnregistrement Enregistrement)
		{
		}
		private static void AssignerElem1(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrNavgt) Enregistrement).CoEnv = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem2(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrNavgt) Enregistrement).CoModSecNav = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem3(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrNavgt) Enregistrement).CoProAccSec = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem4(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrNavgt) Enregistrement).CoFonSec = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem5(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrNavgt) Enregistrement).NmFonSec = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
		}
		private static void AssignerElem6(XdCuValeurEnreg Valeur, XdIEnregistrement Enregistrement)
		{
			((TsEngrNavgt) Enregistrement).CoNivAccSec = XdBaUtilitaireAccesFichier.ToString(Valeur.Valeur);
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


		public TsEngrNavgt()
		{
			CoEnv = string.Empty;
			CoModSecNav = string.Empty;
			CoProAccSec = string.Empty;
			CoFonSec = string.Empty;
			NmFonSec = string.Empty;
			CoNivAccSec = string.Empty;
		}

	}
}
