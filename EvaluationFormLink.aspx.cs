using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data;
using System.Globalization;
using User;
// ****************** 4 linea modificada del original
using System.Data.SqlClient;


public partial class EvaluationForm : System.Web.UI.Page
{
    public string[] _params = new string[] { };
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            NB_save.Text = "Recordá ir grabando la evaluación a medida que la vas cargando.";
        }

        _params = Encryption.Decrypt(Request.Params["."]).Split('.');
        int profile = Convert.ToInt16(_params[1]);
        pId.Value = profile.ToString();

        SetButtonSend();

        SetButtons(profile);
        
    }

    private void SetButtons(int profile)
    {
        bool _visible = false;
        if (profile == 1)
        {
            _visible = ("1,2,5,8,11".IndexOf(_params[2].ToString(), 0) >= 0);
            btnSave.Visible = _visible;
            btnSend.Visible = _visible;
            NB_save.Visible = _visible;
        }
    }

	// ******************* 3 linea modificada del original
	private void SetStatus( int current_profile )
    {
		// **************** 2 linea modificada del original
        //string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        //SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SetAssessmentsSatus]", new object[] { null, Convert.ToInt16(_params[0]), username, _status });


		string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
		//SqlDataReader myReader = SQLHelper.ExecuteReader( Cache["ApplicationDatabase"].ToString( ) , "GetEvaluatedPeople" , new object[] { _params[2] } );
		//myReader.Read( );


		bool exit = false;
		string description = string.Empty;
		string assessment = string.Empty;
		string _letter = string.Empty;
		CultureInfo ci = new CultureInfo( "es-AR" );
		string separator = ci.NumberFormat.NumberDecimalSeparator;


		_params = Encryption.Decrypt( Request.Params["."] ).Split( '.' );

		int groupId = Convert.ToInt16( _params[0] );
		int profile = Convert.ToInt16( _params[1] );


		if( current_profile == 1 ) // Si el que firma es el evaluador envia la evaluacion al concurrente....
		{
			//SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { null , Convert.ToInt16( _params[0] ) , username , 3 } );

			if( profile > 1 )
			{
				username = _params[2];
			}

			for( int i = 0 ; !exit ; i++ )
			{
				Control answer = (Control)this.Master.FindControl( "CPH" ).FindControl( "CFF" ).FindControl( "ctl" + i.ToString( "00" ) );
				exit = answer == null;
				if( !exit )
				{
					if( answer.GetType( ).ToString( ) == "System.Web.UI.WebControls.TextBox" )
					{

						TextBox myControl = (TextBox)answer;

						string[] values = ( (TextBox)answer ).SkinID.Split( '_' );

						if( !myControl.ReadOnly )
						{

							Control radio = (Control)this.Master.FindControl( "CPH" ).FindControl( "CFF" ).FindControl( "ctl" + ( i - 1 ).ToString( "00" ) );

							//if( ( (RadioButton)radio ).Checked )

									if( profile == 1 ) // Si envia el evaluador...
									{
										SqlDataReader ReaderDR = SQLHelper.ExecuteReader( Cache["ApplicationDatabase"].ToString( ) , "EvaluatedHasConcurrent" , new object[] { Convert.ToInt32( ( (TextBox)answer ).SkinID.Split( '_' )[0].ToString( ) ) } );
										ReaderDR.Read( );

										if( ReaderDR.HasRows ) // si el concurrente no es nulo va al concurrente y el envia un email...
										{
											SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 3 } );
										}
										else // si no tiene concurrente se fija si tiene doble reporte
										{

											ReaderDR = SQLHelper.ExecuteReader( Cache["ApplicationDatabase"].ToString( ) , "EvaluatedHasDoubleReport" , new object[] { Convert.ToInt32( ( (TextBox)answer ).SkinID.Split( '_' )[0].ToString( ) ) } );
											ReaderDR.Read( );

											if( ReaderDR.HasRows ) // si tiene doble reporte va al Doble Reporte y el envia un email...
											{
												SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 6 } );
											}
											else // si no tiene doble reporte va a RRHH y le envia un email...
											{
													SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 9 } );
											}
										}
									}
								
						}
						myControl.Dispose( );
					}
				}
			}


		}
		else
		{
			//bool exit = false;
			//string description = string.Empty;
			//string assessment = string.Empty;
			//string _letter = string.Empty;
			//CultureInfo ci = new CultureInfo( "es-AR" );
			//string separator = ci.NumberFormat.NumberDecimalSeparator;


			//_params = Encryption.Decrypt( Request.Params["."] ).Split( '.' );

			//int groupId = Convert.ToInt16( _params[0] );
			//int profile = Convert.ToInt16( _params[1] );
			if( profile > 1 )
			{
				username = _params[2];
			}

			for( int i = 0 ; !exit ; i++ )
			{
				Control answer = (Control)this.Master.FindControl( "CPH" ).FindControl( "CFF" ).FindControl( "ctl" + i.ToString( "00" ) );
				exit = answer == null;
				if( !exit )
				{
					if( answer.GetType( ).ToString( ) == "System.Web.UI.WebControls.TextBox" )
					{

						TextBox myControl = (TextBox)answer;

						string[] values = ( (TextBox)answer ).SkinID.Split( '_' );

						if( !myControl.ReadOnly )
						{

							Control radio = (Control)this.Master.FindControl( "CPH" ).FindControl( "CFF" ).FindControl( "ctl" + ( i - 1 ).ToString( "00" ) );

							if( ( (RadioButton)radio ).Checked )

								if( ( (System.Web.UI.WebControls.CheckBox)( ( (RadioButton)radio ) ) ).Text == "Aprobar" )
								{
									if( profile == 2 ) // Si firma el concurrente...
									{
										SqlDataReader ReaderDR = SQLHelper.ExecuteReader( Cache["ApplicationDatabase"].ToString( ) , "EvaluatedHasDoubleReport" , new object[] { Convert.ToInt32( ( (TextBox)answer ).SkinID.Split( '_' )[0].ToString( ) ) } );
										ReaderDR.Read( );

										if( ReaderDR.HasRows ) // si tiene doble reporte va al Doble Reporte y el envia un email...
										{
											SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 6 } );
										}
										else // si no tiene doble reporte va a RRHH y le envia un email...
										{
											SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 9 } );
										}
									}
									if( profile == 3 ) // Si firma el Doble Reporte va a RRHH y le envia un email...
									{
										SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 9 } );
									}
									if( profile == 5 ) // Si firma RRHH termina la evaluación y le envía un email al evaluador....
									{
										SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 10 } );
									}
								}
								else
								// si se rechaza una evaluacion hay que ver quien la rechaza.................
								{
									switch( profile )
									{
										case 2:
											SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 5 } );
											break;
										case 3:
											SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 8 } );
											break;
										case 5:
											SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SetAssessmentsSatus]" , new object[] { ( (TextBox)answer ).SkinID.Split( '_' )[0] , Convert.ToInt16( _params[0] ) , _params[2] , 11 } );
											break;

									}
								}
						}
						myControl.Dispose( );
					}
				}
			}
		}
    }

    private void SetButtonSend()
    {
        int profile = Convert.ToInt16(_params[1]);
        string lbl = "";
        string css = "bt_sm";
        switch (profile)
        {
            case 1:
                lbl = "Enviar al Concurrente";
                css = "bt_l";
                break;
            case 2:
                lbl = "Enviar";
                break;
            case 3:
                lbl = "Enviar";
                break;
            case 5:
                lbl = "Enviar";
                break;
        }
        btnSend.Text= lbl;
        btnSend.CssClass = css;
        btnSend.CommandArgument = profile.ToString();

    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        //System.Threading.Thread.Sleep(5000);
        Save();
        Button btn = (Button)sender;
        if (btn.CommandArgument != "") {
			SetStatus( Convert.ToInt32( _params[1].ToString( ) ) );

			// ************ 5 linea modificada del original
            //SetStatus(3);
        }

        btn.Dispose();
        
        //NotificationBanner1.Visible = true;
    }

    private void Save()
    {
        int profile = Convert.ToInt16(_params[1]);

        if (profile == 1)
        {
            if (t1.Value == "1")
            {
                SaveMI();
                t1.Value = "";
            }
            if (t2.Value == "1")
            {
                SaveCFF();
                t2.Value = "";
            }
            if (t3.Value == "1")
            {
                SavePM();
                t3.Value = "";
            };
        }
        else
        {
            SaveComments();
        };


        
    }

    private void SaveComments()
    {
        bool exit = false;
        string description = string.Empty;
        string assessment = string.Empty;
        string _letter = string.Empty;
        CultureInfo ci = new CultureInfo("es-AR");
        string separator = ci.NumberFormat.NumberDecimalSeparator;

        string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        string[] _params = new string[] { };

        _params = Encryption.Decrypt(Request.Params["."]).Split('.');

        int groupId = Convert.ToInt16(_params[0]);
        int profile = Convert.ToInt16(_params[1]);
        if (profile > 1)
        {
            username = _params[2];
        }

        for (int i = 0; !exit; i++)
        {
            Control answer = (Control)this.Master.FindControl("CPH").FindControl("CFF").FindControl("ctl" + i.ToString("00"));
            exit = answer == null;
            if (!exit)
            {
                if (answer.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                {

					TextBox myControl = (TextBox)answer;
                    string[] values = myControl.SkinID.Split('_');
                    if (!myControl.ReadOnly)
                    {

						Control radio = (Control)this.Master.FindControl( "CPH" ).FindControl( "CFF" ).FindControl( "ctl" + (i-1).ToString( "00" ) );
						
						if (((RadioButton)radio).Checked )
						
							if (((System.Web.UI.WebControls.CheckBox)(((RadioButton)radio))).Text == "Aprobar")
								SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultComment]", new object[] { values[0], 5, myControl.Text, values[1], "A" });
							else
								SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SaveResultComment]" , new object[] { values[0] , 5 , myControl.Text , values[1] , "R" } );

                    }
                    myControl.Dispose();
                }
            }
        }
    }

    private void SaveMI()
    {
        bool exit = false;
        string description = string.Empty;
        decimal weight = 0;
        decimal result = 0;
        decimal _total = 0;
        string assessment = string.Empty;
        bool _delete = false;
        string _letter = string.Empty;
        CultureInfo ci = new CultureInfo("es-AR");
        string separator = ci.NumberFormat.NumberDecimalSeparator;
        for (int i = 0; !exit; i++)
        {
            Control answer = (Control)this.Master.FindControl("CPH").FindControl("MI").FindControl("ctl" + i.ToString("00"));
            exit = answer == null;
            if (!exit)
            {
                if (answer.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                {
                    TextBox myControl = (TextBox)answer;
                    string[] values = myControl.SkinID.ToUpper().Split('_');

                    switch (values[0])
                    {
                        case "1":
                            description = myControl.Text.ToString();
                            break;
                        case "2":
                            weight = (myControl.Text.ToString() == string.Empty) ? 0 : Convert.ToDecimal(myControl.Text);
                            break;
                        case "4":
                            _delete = (bool)(assessment != values[1]);

                            if (_delete)
                            {
                                HiddenField letter = (HiddenField)this.Master.FindControl("CPH").FindControl("MI").FindControl(myControl.Parent.Parent.Parent.ID + "_l");
                                string[] _values = letter.Value.Trim().Split('_');

                                if (_values.Length != 1)
                                {
                                    _letter = _values[0];
                                    _total = Convert.ToDecimal(_values[1].Replace('.', Convert.ToChar(separator)));
                                }
                                else
                                {
                                    _letter = string.Empty;
                                    _total = 0;
                                }

                                letter.Dispose();
                            }

                            assessment = values[1];
                            result = (myControl.Text.ToString() == string.Empty) ? 0 : Convert.ToDecimal(myControl.Text);
                            SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "SaveResultVariable", new object[] { values[1], 1, values[2], description, weight, result, _letter, _total, _delete });
                            description = string.Empty;
                            weight = 0;
                            result = 0;


                            break;
                    }
                    myControl.Dispose();
                }
                answer.Dispose();
            }
        }
    }

    private void SaveCFF()
    {
        bool exit = false;
        string description = string.Empty;
        int _block = 0;
        string assessment = string.Empty;
        string _letter = string.Empty;
        CultureInfo ci = new CultureInfo("es-AR");
        string separator = ci.NumberFormat.NumberDecimalSeparator;

        string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        string[] _params = new string[] { };

        _params = Encryption.Decrypt(Request.Params["."]).Split('.');

        int groupId = Convert.ToInt16(_params[0]);
        int profile = Convert.ToInt16(_params[1]);
        if (profile > 1)
        {
            username = _params[2];
        }

        for (int i = 0; !exit; i++)
        {
            Control answer = (Control)this.Master.FindControl("CPH").FindControl("CFF").FindControl("ctl" + i.ToString("00"));
            exit = answer == null;
            if (!exit)
            {
                if (answer.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                {
                    TextBox myControl = (TextBox)answer;
                    string[] values = myControl.SkinID.Split('_');
                    if (!myControl.ReadOnly)
                    {
						// ********* 1 linea modificada del original
                        //SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultComment]", new object[] { values[0], 5, myControl.Text, values[1] });
						SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "[SaveResultComment]" , new object[] { values[0] , 5 , myControl.Text , values[1] , "0" } );

                    }
                    myControl.Dispose();
                }

                if (answer.GetType().ToString() == "System.Web.UI.WebControls.RadioButton")
                {
                    RadioButton myControl = (RadioButton)answer;
                    string[] values = myControl.SkinID.ToUpper().Split('_');
                    if (myControl.Checked)
                    {
						// ************ 9 linea agregada al original
						if( values.Length != 1)
						{
							_block = ( Convert.ToInt16( values[1] ) <= 5 ) ? 2 : 3;
							SQLHelper.ExecuteNonQuery( Cache["ApplicationDatabase"].ToString( ) , "SaveResultCompetency" , new object[] { values[0] , _block , values[1] , values[2] , values[3] , true } );
						}
                    }
                    myControl.Dispose();


                }
                answer.Dispose();
            }
        }
        SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "SaveOverallResult", new object[] { username, groupId });
    }

    private void SavePM()
    {
        bool exit = false;
        string _letter = string.Empty;
        object[] values = new object[6];
        int _field = 0;
        for (int i = 0; !exit; i++)
        {
            Control answer = (Control)this.Master.FindControl("CPH").FindControl("PM").FindControl("ctl" + i.ToString("00"));
            exit = answer == null;
            if (!exit)
            {
                if (answer.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                {
                    TextBox myControl = (TextBox)answer;
                    if (!myControl.ReadOnly)
                    {
                        values[_field] = myControl.Text;
                        _field++;

                        if (_field == 6)
                        {
                            SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultImprovementPlan]", new object[] { Convert.ToInt16(myControl.SkinID.ToString()), 3, values[0], values[1], values[2], values[3], values[4], values[5] });
                            Array.Clear(values, 0, values.Length);
                            _field = 0;
                        }
                    }
                    myControl.Dispose();
                }
                answer.Dispose();
            }
        }
    }
}
