using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data;
using System.Globalization;
using User;
using System.Data.SqlClient;
using System.Web;


/// <summary>
/// Summary description for PrintForm
/// </summary>
public class PrintForm
{
	#region Variables
	string _apellidoynombrevaluado = string.Empty;
	string _puestoevaluado = string.Empty;
	string _apellidoynombreevaluador = string.Empty;
	string _direccionevaluado = string.Empty;
	string _areaevaluado = string.Empty;
	string _apellidoynombreconcurrent = string.Empty;
	string _apellidoynombredoblereporte = string.Empty;
	string _textoresultados1 = string.Empty;
	string _textoresultados2 = string.Empty;
	string _resultados1 = string.Empty;
	string _resultados2 = string.Empty;
	string _calificacionglobal = string.Empty;
	string _calificacionglobalgestion = string.Empty;
	string _paraquemejorar1 = string.Empty;
	string _paraquemejorar2 = string.Empty;
	string _paraquemejorar3 = string.Empty;
	string _comomejorar1 = string.Empty;
	string _comomejorar2 = string.Empty;
	string _comomejorar3 = string.Empty;
	string _comentariosevaluador = string.Empty;
	string _comentariosconcurrente = string.Empty;
	string _comentariosdoblereporte = string.Empty;
	string _comentariosrrhh = string.Empty;
	string[] _competenciafemsadescription = new string[20];
	string[] _competenciafemsanotadescription = new string[20];
	string[] _calificacioncompetenciafemsa = new string[20];
	string[] _competenciasfuncionalesdescription = new string[20];
	string[] _competenciasfuncionalesnotadescription = new string[20];
	string[] _calificacioncompetenciasfuncionales = new string[20];
	string[] _indicadordegestion = new string[12];
	string[] _ponderacion = new string[12];
	string[] _unidaddemedida = new string[12];
	string[] _resultadologrado = new string[12] ;
	string[] _calificaciongestion = new string[12];
	string[] _displaymode = new string[20] { "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" };
	string[] _displaymodeFEMSAcompetency = new string[20] { "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" };
	string[] _displaymodefunctionalcompetency = new string[20] { "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" , "none" };
	string _fechae = string.Empty;
	string _fechac = string.Empty;
	string _fechadr = string.Empty;
	string _dispresultados1 = string.Empty;
	string _dispresultados2 = string.Empty;
	string _aclaracionvisible = string.Empty;
    bool _esjefe = false;

	#endregion
	#region Properties

	public string ApellidoyNombreEvaluado { get { return _apellidoynombrevaluado; }}
	public string PuestoEvaluado{ get { return _puestoevaluado; }}
	public string ApellidoyNombreEvaluador{ get { return _apellidoynombreevaluador; }}
	public string DireccionEvaluado{ get { return _direccionevaluado; }}
	public string AreaEvaluado { get { return _areaevaluado; } }
	public string ApellidoyNombreConcurrente { get { return _apellidoynombreconcurrent; } }
	public string ApellidoyNombreDobleReporte { get { return _apellidoynombredoblereporte; } }
	public string TextoResultados1 { get { return _textoresultados1; } }
	public string TextoResultados2{ get { return _textoresultados2; }}
	public string Resultados1{ get { return _resultados1; }}
	public string Resultados2{ get { return _resultados2; }}
	public string CalificacionGlobal{ get { return _calificacionglobal; }}
	public string CalificacionGlobalGestion { get { return _calificacionglobalgestion; } }
	public string ParaQueMejorar1 { get { return _paraquemejorar1; } }
	public string ParaQueMejorar2{ get { return _paraquemejorar2; }}
	public string ParaQueMejorar3{ get { return _paraquemejorar3; }}
	public string ComoMejorar1{ get { return _comomejorar1; }}
	public string ComoMejorar2{ get { return _comomejorar2; }}
	public string ComoMejorar3{ get { return _comomejorar3; }}
	public string ComentariosEvaluador{ get { return _comentariosevaluador; }}
	public string ComentariosConcurrente{ get { return _comentariosconcurrente; }}
	public string ComentariosDobleReporte { get { return _comentariosdoblereporte; } }
	public string ComentariosRRHH { get { return _comentariosrrhh; } }
	public string[] CompetenciasFEMSADescription { get { return _competenciafemsadescription; } }
	public string[] CompetenciasFEMSANotaDescription { get { return _competenciafemsanotadescription; } }
	public string[] CalificacionCompetenciaFemsa { get { return _calificacioncompetenciafemsa; } }
	public string[] CompetenciasFuncionalesDescription { get { return _competenciasfuncionalesdescription; } }
	public string[] CompetenciasFuncionalesNotaDescription { get { return _competenciasfuncionalesnotadescription; } }
	public string[] CalificacionCompetenciasFuncionales { get { return _calificacioncompetenciasfuncionales; } }
	public string[] IndicadordeGestion{ get { return _indicadordegestion; }}
	public string[] Ponderacion{ get { return _ponderacion; }}
	public string[] UnidadDeMedida{ get { return _unidaddemedida; }}
	public string[] ResultadoLogrado{ get { return _resultadologrado; }}
	public string[] Calificacion { get { return _calificaciongestion; } }
	public string[] DisplayMode{ get { return _displaymode; } }
	public string[] DisplayModeFEMSACompetency { get { return _displaymodeFEMSAcompetency; } }
	public string[] DisplayModeFunctionalCompetency { get { return _displaymodefunctionalcompetency; } }
	public string FechaE { get { return _fechae; } }
	public string FechaC { get { return _fechac; } }
	public string FechaDR { get { return _fechadr; } }
	public string DispResultados1 { get { return _dispresultados1; } }
	public string DispResultados2 { get { return _dispresultados2; } }
	public string AclaracionVisible { get { return _aclaracionvisible; } }

	#endregion
	#region Methods

		#region SetData
		public void SetData( int evaluationid )
		{
			try
			{
				SqlDataReader myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationGeneralData" , new object[] { evaluationid } );

			
				while( myReader.Read( ) )
				{
					_apellidoynombrevaluado = myReader.GetValue( 0 ).ToString();
					_puestoevaluado = myReader.GetValue( 1 ).ToString();
					_apellidoynombreevaluador = myReader.GetValue(2).ToString();
					_direccionevaluado = myReader.GetValue(3).ToString();
					_areaevaluado = myReader.GetValue( 9 ).ToString( );
					if( myReader.GetValue( 4 ).ToString( ) == string.Empty )
						_apellidoynombreconcurrent = "No tiene ";
					else
						_apellidoynombreconcurrent = myReader.GetValue( 4 ).ToString();
					if( myReader.GetValue( 5 ).ToString( ) == string.Empty )
						_apellidoynombredoblereporte = "No tiene ";
					else
						_apellidoynombredoblereporte = myReader.GetValue( 5 ).ToString( );

				}
				

				myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "[GetEvaluationGlobalResult]" , new object[] { evaluationid } );


				while( myReader.Read( ) )
				{
					_calificacionglobal = myReader.GetValue( 3 ).ToString( );
				}

				_dispresultados1 = "block";
				_dispresultados2 = "block";

				myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "[IsChief]" , new object[] { evaluationid } );

				myReader.Read( );

				//if( myReader.GetValue( 0 ).ToString( ) == "0" )
				//{

				//    _textoresultados1 = "Calificación Competencias FEMSA : ";
				//    _textoresultados2 = "Calificación Competencias Funcionales : ";
				//    myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "[GetBlockResult]" , new object[] { evaluationid } );


				//    myReader.Read( );
				//    _resultados1 = myReader.GetValue( 2 ).ToString( ) + " - " + myReader.GetValue( 3 ).ToString( );
				//    myReader.Read( );
				//    _resultados2 = myReader.GetValue( 2 ).ToString( ) + " - " + myReader.GetValue( 3 ).ToString( );
				//    _aclaracionvisible = "<table > <tr> <td style=\"color: #800000; font-style: italic; font-size: small;\" > <b>Aclaración importante :</b> La calificación que surje de estos 'Resultados de gestión', NO tienen peso en la Evaluación, es decir, no influyen en el resultado final de la Gestión del Desempeño</td> </tr> </table>";

				//}
				//else
				//{

				//    myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "[GetCompetenciesResult]" , new object[] { evaluationid } );

				//    _textoresultados2 = "Calificación Competencias : ";
				//    _textoresultados1 = "Calificación Indicadores de Gestión : ";

				//    myReader.Read( );
				//    _resultados2 = myReader.GetValue( 0 ).ToString( ) + " - " + myReader.GetValue( 1 ).ToString( );

				//    myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "[GetGestionResult]" , new object[] { evaluationid } );
				//    myReader.Read( );
				//    _resultados1 = myReader.GetValue( 0 ).ToString( ) + " - " + myReader.GetValue( 1 ).ToString( );
				//    _aclaracionvisible = "";
				//}

				// NUEVO ENCABEZADO PARA TODOS IGUAL

				if( myReader.GetValue( 0 ).ToString( ) == "0" )
				{
					_aclaracionvisible = "<table > <tr> <td style=\"color: #800000; font-style: italic; font-size: small;\" > <b>Aclaración importante :</b> La calificación que surje de estos 'Resultados de gestión', NO tienen peso en la Evaluación, es decir, no influyen en el resultado final de la Gestión del Desempeño</td> </tr> </table>";
                    _esjefe = false;
				}
				else
				{
					_aclaracionvisible = "<table > <tr> <td style=\"color: #800000; font-style: italic; font-size: small;\" > <b>Aclaración importante :</b> Los indicadores de Gestión (Factores Críticos) del nivel 'Jefes', ya han sido cargados y evaluados a través  del Sistema TOPS, por tal motivo no serán evaluados en la herramienta Gestión del desempeño.  </td> </tr> </table>";
                    _esjefe = true;
				}


				_textoresultados1 = "Calificación Competencias FEMSA : ";
				_textoresultados2 = "Calificación Competencias Funcionales : ";
				myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "[GetBlockResult]" , new object[] { evaluationid } );
				myReader.Read( );
				_resultados1 = myReader.GetValue( 2 ).ToString( ) + " - " + myReader.GetValue( 3 ).ToString( );
				myReader.Read( );
				_resultados2 = myReader.GetValue( 2 ).ToString( ) + " - " + myReader.GetValue( 3 ).ToString( );


				// FIN DE NUEVO ENCABEZADO

				myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationImprovementPlanComments" , new object[] { evaluationid } );


				while( myReader.Read( ) )
				{
					_paraquemejorar1 = myReader.GetValue( 0 ).ToString( );
					_paraquemejorar2 = myReader.GetValue( 2 ).ToString( );
					//_paraquemejorar3 = myReader.GetValue( 4 ).ToString( );
					_comomejorar1 = myReader.GetValue( 1 ).ToString( );
					_comomejorar2 = myReader.GetValue( 3 ).ToString( );
					_comomejorar3 = myReader.GetValue( 5 ).ToString( );
				}

				myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationComments" , new object[] { evaluationid } );


				while( myReader.Read( ) )
				{

					_comentariosevaluador = myReader.GetValue( 0 ).ToString();
					if( myReader.GetValue( 1 ).ToString( ) != "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ..........." )
						_comentariosconcurrente = myReader.GetValue( 1 ).ToString( );
					else
						_comentariosconcurrente = "";

					if( myReader.GetValue( 2 ).ToString( ) != "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ..........." )
						_comentariosdoblereporte = myReader.GetValue( 2 ).ToString( );
					else
						_comentariosdoblereporte = "";

                    //if( myReader.GetValue( 3 ).ToString( ) != "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ..........." )
						_calificacionglobalgestion = myReader.GetValue( 3 ).ToString( );
                    //else
                    //    _calificacionglobalgestion = "";

					if( myReader.GetValue( 7 ).ToString( ) != "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ..........." )
						_comentariosrrhh = myReader.GetValue( 7 ).ToString( );
					else
						_comentariosrrhh = "";

		
					if( myReader.GetValue( 4 ).ToString( )  != string.Empty)
					if( Convert.ToDateTime( myReader.GetValue( 4 ).ToString( ) ) == DateTime.MinValue )
						_fechae = myReader.GetValue( 4 ).ToString( );
					else
						_fechae = "";
					if( myReader.GetValue( 5 ).ToString( ) != string.Empty )
						if( Convert.ToDateTime( myReader.GetValue( 5 ).ToString( ) ) == DateTime.MinValue )
						_fechac = myReader.GetValue( 5 ).ToString( );
					else
						_fechac = "";
					if( myReader.GetValue( 6 ).ToString( ) != string.Empty )
						if( Convert.ToDateTime( myReader.GetValue( 6 ).ToString( ) ) == DateTime.MinValue )
						_fechadr = myReader.GetValue( 6 ).ToString( );
					else
						_fechae = "";
				}

				myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationVariableResult" , new object[] { evaluationid } );

				int indice = 0;
                double suma = 0;
				bool otraponderacion = false;

				while( myReader.Read( ) )
				{
					_indicadordegestion[indice] = myReader.GetValue( 0 ).ToString( );

					if( myReader.GetValue( 1 ).ToString( ) == "0" )
						otraponderacion = true;

					_ponderacion[indice] = myReader.GetValue( 1 ).ToString( );
					_resultadologrado[indice] = myReader.GetValue( 2 ).ToString( );
                    suma += (Convert.ToDouble(myReader.GetValue(2)) / 10000 * Convert.ToDouble(myReader.GetValue(1)));
					_unidaddemedida[indice] = "% cumpl";
					_displaymode[indice] = "";
					indice++;
				}
                if (_esjefe)
                {
                    _calificacionglobalgestion = "--";
                }
                else
                {
                    if (suma >= 0 && suma <= 0.799)
                    {
                        _calificacionglobalgestion = "1";
                    }
                    else if (suma <= 0.849)
                    {
                        _calificacionglobalgestion = "2";
                    }
                    else if (suma <= 0.929)
                    {
                        _calificacionglobalgestion = "3";
                    }
                    else if (suma <= 0.979)
                    {
                        _calificacionglobalgestion = "4";
                    }
                    else
                        _calificacionglobalgestion = "5";
                }
				indice = 0;
				if( otraponderacion )
				{
					myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationOtherPonderation" , new object[] { evaluationid } );
					while( myReader.Read( ) )
					{
						_ponderacion[indice] = myReader.GetValue( 0 ).ToString( );
						_indicadordegestion[indice] = myReader.GetValue( 1 ).ToString( );
						indice++;
					}
				}
																											
				myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationCompetencyFEMSAResults" , new object[] { evaluationid } );

				indice = 0;
				while( myReader.Read( ) )
				{
					_competenciafemsadescription[indice] = myReader.GetValue( 0 ).ToString();
					_competenciafemsanotadescription[indice] = myReader.GetValue( 3 ).ToString( );

					//switch (myReader.GetValue( 2 ).ToString( ))
					//{
					//    case "A":
					//        _calificacioncompetenciafemsa[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 5 * indice ).ToString( ) + "\" checked=\"checked\"   disabled=\"disabled\" name=\"R" + indice.ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 1 ).ToString( ) + "\"  disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 2 ).ToString( ) + "\"    disabled=\"disabled\" name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 5 * indice ) + 3 ).ToString( ) + "\"  disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 5 * indice ) + 4 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//    case "B":
					//        _calificacioncompetenciafemsa[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 5 * indice ).ToString( ) + "\"     disabled=\"disabled\" name=\"R" + indice.ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 1 ).ToString( ) + "\" checked=\"checked\"   disabled=\"disabled\" name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 2 ).ToString( ) + "\"   disabled=\"disabled\"   name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 5 * indice ) + 3 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 5 * indice ) + 4 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//    case "C":
					//        _calificacioncompetenciafemsa[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 5 * indice ).ToString( ) + "\"     disabled=\"disabled\" name=\"R" + indice.ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 1 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 2 ).ToString( ) + "\" checked=\"checked\"  disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 5 * indice ) + 3 ).ToString( ) + "\"   disabled=\"disabled\"   name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 5 * indice ) + 4 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//    case "C+":
					//        _calificacioncompetenciafemsa[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 5 * indice ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 1 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 2 ).ToString( ) + "\"   disabled=\"disabled\"   name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 5 * indice ) + 3 ).ToString( ) + "\" checked=\"checked\"   disabled=\"disabled\" name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 5 * indice ) + 4 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//    case "D":
					//        _calificacioncompetenciafemsa[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 5 * indice ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 1 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 5 * indice ) + 2 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 5 * indice ) + 3 ).ToString( ) + "\"    disabled=\"disabled\"  name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 5 * indice ) + 4 ).ToString( ) + "\" checked=\"checked\"   disabled=\"disabled\" name=\"R" + indice.ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//}
					_calificacioncompetenciafemsa[indice] = myReader.GetValue( 2 ).ToString( );
					_displaymodeFEMSAcompetency[indice] = "";
					indice++;
				}

				myReader = SQLHelper.ExecuteReader( HttpRuntime.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationFunctionalCompetencyResults" , new object[] { evaluationid } );

				indice = 0;
				while( myReader.Read( ) )
				{
					_competenciasfuncionalesdescription[indice] = myReader.GetValue( 0 ).ToString( );
					_competenciasfuncionalesnotadescription[indice] = myReader.GetValue( 3 ).ToString( );
					//switch( myReader.GetValue( 2 ).ToString( ) )
					//{
					//    case "A":
					//        _calificacioncompetenciasfuncionales[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 101 * indice ).ToString( ) + "\" checked=\"checked\"  disabled=\"disabled\" name=\"R" + (101 + indice).ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 1 ).ToString( ) + "\" disabled=\"disabled\"  name=\"R" + (101+indice).ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 2 ).ToString( ) + "\"  disabled=\"disabled\"  name=\"R" + (101+ indice).ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 101 * indice ) + 3 ).ToString( ) + "\" disabled=\"disabled\"  name=\"R" + (101+indice).ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 101 * indice ) + 4 ).ToString( ) + "\"   disabled=\"disabled\"  name=\"R" + (101+indice).ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//    case "B":
					//        _calificacioncompetenciasfuncionales[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 101 * indice ).ToString( ) + "\"   disabled=\"disabled\"  name=\"R" + ( 101 + indice ).ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 1 ).ToString( ) + "\" checked=\"checked\" name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 2 ).ToString( ) + "\"  disabled=\"disabled\"   name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 101 * indice ) + 3 ).ToString( ) + "\"  disabled=\"disabled\"   name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 101 * indice ) + 4 ).ToString( ) + "\"   disabled=\"disabled\"  name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//    case "C":
					//        _calificacioncompetenciasfuncionales[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 101 * indice ).ToString( ) + "\"   disabled=\"disabled\"  name=\"R" + ( 101 + indice ).ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 1 ).ToString( ) + "\"    disabled=\"disabled\" name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 2 ).ToString( ) + "\" checked=\"checked\" disabled=\"disabled\"  name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 101 * indice ) + 3 ).ToString( ) + "\"  disabled=\"disabled\"   name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 101 * indice ) + 4 ).ToString( ) + "\"   disabled=\"disabled\"  name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//    case "C+":
					//        _calificacioncompetenciasfuncionales[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 101 * indice ).ToString( ) + "\" disabled=\"disabled\"  name=\"R" + ( 101 + indice ).ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 1 ).ToString( ) + "\" disabled=\"disabled\"    name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 2 ).ToString( ) + "\"  disabled=\"disabled\"   name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 101 * indice ) + 3 ).ToString( ) + "\" checked=\"checked\"  disabled=\"disabled\" name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 101 * indice ) + 4 ).ToString( ) + "\"  disabled=\"disabled\"   name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//    case "D":
					//        _calificacioncompetenciasfuncionales[indice] = " <td></td><td></td><td></td><td></td><td></td><td><input id=\"Radio" + ( 101 * indice ).ToString( ) + "\"   disabled=\"disabled\"  name=\"R" + ( 101 + indice ).ToString( ) + "\" type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 1 ).ToString( ) + "\"    disabled=\"disabled\" name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td><td><input id=\"Radio" + ( ( 101 * indice ) + 2 ).ToString( ) + "\"   disabled=\"disabled\"  name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td> <td><input id=\"Radio" + ( ( 101 * indice ) + 3 ).ToString( ) + "\"  disabled=\"disabled\"   name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td>  <td><input id=\"Radio" + ( ( 101 * indice ) + 4 ).ToString( ) + "\" checked=\"checked\"  disabled=\"disabled\" name=\"R" + ( 101 + indice ).ToString( ) + "\"  type=\"radio\"   /></td> ";
					//        break;
					//}

					_calificacioncompetenciasfuncionales[indice] = myReader.GetValue( 2 ).ToString( );
					_displaymodefunctionalcompetency[indice] = "";
					indice++;
				}

			}
			catch 
			{
			}
		}

		#endregion
	#endregion
}
