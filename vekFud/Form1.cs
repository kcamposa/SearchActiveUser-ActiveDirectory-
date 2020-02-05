using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// ---------------------------- necessary ----------------------------
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Text.RegularExpressions;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)] 
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]
// --------------------------------------------------------------------

namespace vekFud
{
    public partial class frmUserAD : Form
    {
        public frmUserAD()
        {
            InitializeComponent();
        }

        // ---------------------------- global variables ----------------------------
        List<string> workers;
        List<string> NewOrderWorkers;
        List<string> UserNamesCreated;
        List<string> ActiveUsers;

        public DirectorySearcher dirSearch = null;
        // ----------------------------------------------------------------------


        private void Form1_Load(object sender, EventArgs e) // load extra data first 
        {
            txtUser.Enabled = false;
            txtPassword.Enabled = false;
            btnStop.Enabled = false;

            txtUser.BackColor = Color.LightGray;
            txtPassword.BackColor = Color.LightGray;
            txtDomain.BackColor = Color.LightGray;
            txtPath.BackColor = Color.LightGray;
            btnStop.BackColor = Color.LightGray;

            btnChooseFile.Select();
            radioButton1.Checked = true;

            txtDomain.Text = getDomainName();
        }

        private void btnChooseFile_Click(object sender, EventArgs e) // choose txt file 
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); // create an instance
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"; // types of files to work

            string pathFile = string.Empty;

            if (radioButton1.Checked == true) // -------------- LIST IN DISORDER RADIO BUTTON 1
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pathFile = openFileDialog.FileName;     // path the file name
                    txtPath.Text = pathFile; //only visualization path name    

                    bool names = loadNames(pathFile);
                    if (names == true)
                    {
                        var result = workers.Select(s => new { value = s }).ToList();  // fill the datagrid with the correct name
                        dataGridView1.DataSource = result;

                        if (dataGridView1.Rows.Count > 0) // if the datagrid1 is loaded, datagrid2 will loaded
                        {
                            orderNames();
                            var result1 = NewOrderWorkers.Select(s => new { value = s }).ToList(); // fill the datagrid with the correct name
                            dataGridView2.DataSource = result1;

                            if (dataGridView2.Rows.Count > 0)
                            {
                                createUserNames();
                                var result2 = UserNamesCreated.Select(s => new { value = s }).ToList(); // fill the datagrid with the correct name
                                dataGridView3.DataSource = result2;

                                btnStop.Enabled = true;
                                btnStop.BackColor = Color.Red;
                            }
                            else
                            {
                                MessageBox.Show("The Txt Users can't showed");
                            }

                            //DISABLE/enable OPTIONS 
                            btnSearch.Enabled = true;
                            txtUser.Enabled = true;
                            txtUser.BackColor = Color.White;
                            txtPassword.Enabled = true;
                            txtPassword.BackColor = Color.White;
                            btnChooseFile.Enabled = false;
                            radioButton1.Enabled = false;
                            radioButton2.Enabled = false;

                        }
                        else
                        {
                            MessageBox.Show("The Txt Users can't showed");
                        }
                    }
                    else
                    {
                        MessageBox.Show("We can't load the txt users, please check the txt file first.");
                    }

                }
            } else if (radioButton2.Checked == true) // -------------- LIST IN ORDER RADIO BUTTON 2
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pathFile = openFileDialog.FileName;     // path the file name
                    txtPath.Text = pathFile; //only visualization path name 

                    bool names = loadNames(pathFile);
                    if (names == true)
                    {
                        var result = workers.Select(s => new { value = s }).ToList();  // fill the datagrid with the correct name
                        dataGridView1.DataSource = result;

                        if (dataGridView1.Rows.Count > 0) // if the datagrid1 is loaded, datagrid2 will loaded
                        {
                            orderNames_orderedList();
                            var result1 = NewOrderWorkers.Select(s => new { value = s }).ToList(); // fill the datagrid with the correct name
                            dataGridView2.DataSource = result1;

                            if (dataGridView2.Rows.Count > 0)
                            {
                                createUserNames();
                                var result2 = UserNamesCreated.Select(s => new { value = s }).ToList(); // fill the datagrid with the correct name
                                dataGridView3.DataSource = result2;

                                btnStop.Enabled = true;
                                btnStop.BackColor = Color.Red;
                            }
                            else
                            {
                                MessageBox.Show("The Txt Users can't showed");
                            }
                            //DISABLE/enable OPTIONS 
                            btnSearch.Enabled = true;
                            txtUser.Enabled = true;
                            txtUser.BackColor = Color.White;
                            txtPassword.Enabled = true;
                            txtPassword.BackColor = Color.White;
                            btnChooseFile.Enabled = false;
                            radioButton1.Enabled = false;
                            radioButton2.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("The Txt Users can't showed");
                        }
                    }
                    else
                    {
                        MessageBox.Show("We can't load the txt users, please check the txt file first.");
                    }
                }
            }
        }      

        private void btnSearch_Click(object sender, EventArgs e) // search each user 
        {
            if ( txtUser.Text.Trim().Length != 0 && txtPassword.Text.Trim().Length != 0 )
            {
                if (correctUserAdmin(txtDomain.Text, txtUser.Text, txtPassword.Text) != false) // check the user and password admin
                {
                    searchUsers(txtUser.Text, txtPassword.Text, txtDomain.Text);
                    if (ActiveUsers.Count > 0)
                    {
                        var result = ActiveUsers.Select(s => new { value = s }).ToList();  // fill the datagrid with the correct name
                        dataGridView4.DataSource = result;
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect credential Admin, please check the user or password.");
                }
            }
            else
            {
                MessageBox.Show("Please, put your credentials admin to search active users in AD.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //clean list and datagrid
            if (workers != null)
            {
                workers.Clear();
            }
            if (NewOrderWorkers != null)
            {
                NewOrderWorkers.Clear();
            }
            if(UserNamesCreated != null)
            {
                UserNamesCreated.Clear();
            }
            if (ActiveUsers != null )
            {
                ActiveUsers.Clear();
            }
            if (dataGridView1.Rows.Count > 0 )
            {
                dataGridView1.DataSource = null;
            }
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.DataSource = null;
            }
            if (dataGridView3.Rows.Count > 0)
            {
                dataGridView3.DataSource = null;
            }
            if (dataGridView4.Rows.Count > 0)
            {
                dataGridView4.DataSource = null;
            }         
            
            //disable options
            txtUser.Enabled = false;
            txtPassword.Enabled = false;
            btnSearch.Enabled = false;
            txtPath.Text = "";
            btnStop.Enabled = false;
            btnStop.BackColor = Color.LightGray;

            //enable options
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            btnChooseFile.Enabled = true;
        }



        //private external methods(in order of execution)
        private Boolean loadNames(string pathfile) // read txt file and load names, here is the try catch 
        {

            bool load = false;
            StreamReader fichero;
            workers = new List<string>();
            try
            {
                fichero = File.OpenText( pathfile );
                string linea = "";
                while ( ( linea = fichero.ReadLine() ) != null )
                {
                    workers.Add(linea);
                }
                if ( workers.Count == 0 )
                {
                    return load;
                }
                else
                {
                    load = true;
                }

                fichero.Close();
                return load;
            }
            catch (Exception)
            {
                MessageBox.Show("We can't read the file, please check the txt file. >> try catch exception <<");
                throw;
            }
        }

        private void orderNames() // put in order the names, name + second name + first lastname + second last name 
        {
            NewOrderWorkers = new List<string>();
            string word = "";
            for (int i = 0; i < workers.Count; i++)
            {

                word = string.Join( " ", workers[i].ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ); // remove double spaces in the string
                string[] splitWord = word.Split( ' ' ); // split the string 
                
                if ( splitWord.Length == 4 )
                { 
                    NewOrderWorkers.Add( 
                          char.ToUpper( splitWord[2][0] ) + splitWord[2].Substring(1) + " " // capitalize and put in order the name
                        + char.ToUpper( splitWord[3][0] ) + splitWord[3].Substring(1) + " "  
                        + char.ToUpper( splitWord[0][0] ) + splitWord[0].Substring(1) + " "   
                        + char.ToUpper( splitWord[1][0] ) + splitWord[1].Substring(1) 
                    ); 

                } else if ( splitWord.Length == 3 )
                {
                    NewOrderWorkers.Add(
                          char.ToUpper( splitWord[2][0] ) + splitWord[2].Substring(1) + " " // capitalize and put in order the name
                        + char.ToUpper( splitWord[0][0] ) + splitWord[0].Substring(1) + " " 
                        + char.ToUpper( splitWord[1][0] ) + splitWord[1].Substring(1) 
                    );
                } else if ( splitWord.Length == 2 )
                {
                    NewOrderWorkers.Add(
                          char.ToUpper( splitWord[1][0] ) + splitWord[1].Substring(1) + " " // capitalize and put in order the name
                        + char.ToUpper( splitWord[0][0] ) + splitWord[0].Substring(1)
                    );
                }
                else
                {
                    NewOrderWorkers.Add( "We can't read the user >" + workers[i] + "< please check the file." );
                }
            }

        }

        private void orderNames_orderedList() // ordered list, just capitalize
        {
            NewOrderWorkers = new List<string>();
            string word = "";
            for (int i = 0; i < workers.Count; i++)
            {

                word = string.Join(" ", workers[i].ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)); // remove double spaces in the string
                string[] splitWord = word.Split(' '); // split the string 

                if (splitWord.Length == 4)
                {
                    NewOrderWorkers.Add(
                          char.ToUpper(splitWord[0][0]) + splitWord[0].Substring(1) + " " // capitalize and put in order the name
                        + char.ToUpper(splitWord[1][0]) + splitWord[1].Substring(1) + " "
                        + char.ToUpper(splitWord[2][0]) + splitWord[2].Substring(1) + " "
                        + char.ToUpper(splitWord[3][0]) + splitWord[3].Substring(1)
                    );

                }
                else if (splitWord.Length == 3)
                {
                    NewOrderWorkers.Add(
                          char.ToUpper(splitWord[0][0]) + splitWord[0].Substring(1) + " " // capitalize and put in order the name
                        + char.ToUpper(splitWord[1][0]) + splitWord[1].Substring(1) + " "
                        + char.ToUpper(splitWord[2][0]) + splitWord[2].Substring(1)
                    );
                }
                else if (splitWord.Length == 2)
                {
                    NewOrderWorkers.Add(
                          char.ToUpper(splitWord[0][0]) + splitWord[0].Substring(1) + " " // capitalize and put in order the name
                        + char.ToUpper(splitWord[1][0]) + splitWord[1].Substring(1)
                    );
                }
                else
                {
                    NewOrderWorkers.Add("We can't read the user >" + workers[i] + "< please check the file.");
                }
            }

        }

        private string getDomainName() // get domain name 
        {
            try
            {
                return Domain.GetComputerDomain().ToString().ToLower();
            }
            catch (Exception e)
            {
                e.Message.ToString();
                return string.Empty;
            }
        }

        private void createUserNames() // create the possible usernames to search 
        {
            UserNamesCreated = new List<string>();
            string NameLastname = "";

            for (int i = 0; i < NewOrderWorkers.Count; i++)
            {
                NameLastname = NewOrderWorkers[i].ToLower();
                string[] splitNameLastname = NameLastname.Split(' '); // split the name for spaces

                if ( splitNameLastname.Length == 4 ) // concat possible AD usernames 
                {                                                                // remove ñ ''                          
                    UserNamesCreated.Add( splitNameLastname[0].Substring(0, 1) + Regex.Replace(splitNameLastname[2].Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "") + splitNameLastname[3].Substring(0, 1) );
                    UserNamesCreated.Add( splitNameLastname[0].Substring(0, 1) + Regex.Replace(splitNameLastname[2].Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "") + splitNameLastname[3].Substring(0, 2) );
                    if (splitNameLastname[0].Substring(0, 1) != splitNameLastname[1].Substring(0, 1)) // initial first name != initial second name
                    {
                        UserNamesCreated.Add(splitNameLastname[1].Substring(0, 1) + Regex.Replace(splitNameLastname[2].Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "") + splitNameLastname[3].Substring(0, 1));
                        UserNamesCreated.Add(splitNameLastname[1].Substring(0, 1) + Regex.Replace(splitNameLastname[2].Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "") + splitNameLastname[3].Substring(0, 2));
                    }                  
                } else if ( splitNameLastname.Length == 3 )
                {
                    UserNamesCreated.Add( splitNameLastname[0].Substring(0, 1) + Regex.Replace(splitNameLastname[1].Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "") + splitNameLastname[2].Substring(0, 1) );
                } else if ( splitNameLastname.Length == 2 )
                {
                    UserNamesCreated.Add( splitNameLastname[0].Substring(0, 1) + Regex.Replace(splitNameLastname[1].Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""));
                }
            }
        }       

        private bool correctUserAdmin(string activeDirectoryServerDomain, string username, string password) // test user admin
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://" + activeDirectoryServerDomain, username + "@" + activeDirectoryServerDomain, password, AuthenticationTypes.Secure);
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        private void searchUsers(string username, string passowrd, string domain)
        {
            ActiveUsers = new List<string>();

            SearchResult rs = null;

            for (int i = 0; i < UserNamesCreated.Count; i++)
            {
                rs = SearchUserByUserName(GetDirectorySearcherUS(username, passowrd, domain), UserNamesCreated[i]);
                if (rs != null)
                {
                    ActiveUsers.Add( UserNamesCreated[i] + "  /  " // user properties
                        + rs.GetDirectoryEntry().Properties["givenName"].Value.ToString() + " " + rs.GetDirectoryEntry().Properties["sn"].Value.ToString() + "  /  "
                        + rs.GetDirectoryEntry().Properties["title"].Value.ToString() );
                }
            }
        }

        private SearchResult SearchUserByUserName(DirectorySearcher ds, string username) // I need this method to search the user
        {
            ds.Filter = "(&((&(objectCategory=Person)(objectClass=User)))(samaccountname=" + username + "))";

            ds.SearchScope = SearchScope.Subtree;
            ds.ServerTimeLimit = TimeSpan.FromSeconds(90);

            SearchResult userObject = ds.FindOne();

            if (userObject != null)
                return userObject;
            else
                return null;
        }

        private DirectorySearcher GetDirectorySearcherUS(string username, string passowrd, string domain) // search directory
        {
            if (dirSearch == null)
            {
                try
                {
                    dirSearch = new DirectorySearcher(
                        new DirectoryEntry("LDAP://" + domain, username, passowrd));
                }
                catch (DirectoryServicesCOMException e)
                {
                    MessageBox.Show("Connection Creditial is Wrong!!!, please Check.", "Erro Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Message.ToString();
                }
                return dirSearch;
            }
            else
            {
                return dirSearch;
            }
        }

//*************************************************************************************************************************************
    }
}
