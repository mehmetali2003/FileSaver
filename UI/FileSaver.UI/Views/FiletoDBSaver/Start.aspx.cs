using FileSaver.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FileSaver.UI.Views.FiletoDBSaver
{
    public partial class Start : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gridPerson_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //Set the edit index.
            gridPersonData.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void gridPerson_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //Reset the edit index.
            gridPersonData.EditIndex = -1;
            BindData();
        }

        protected void gridPerson_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable personDataTable = this.ViewState["GridPersonDataSource"] as DataTable;
            GridViewRow gridViewRow = gridPersonData.Rows[e.RowIndex];
            DataRow dataRow = personDataTable.Rows[gridViewRow.DataItemIndex];
            dataRow["FirstName"] = ((TextBox)(gridViewRow.Cells[1].Controls[0])).Text;
            dataRow["Surname"] = ((TextBox)(gridViewRow.Cells[2].Controls[0])).Text;
            dataRow["Age"] = ((TextBox)(gridViewRow.Cells[3].Controls[0])).Text;
            dataRow["Sex"] = ((TextBox)(gridViewRow.Cells[4].Controls[0])).Text;
            dataRow["Mobile"] = ((TextBox)(gridViewRow.Cells[5].Controls[0])).Text;
            dataRow["Active"] = ((TextBox)(gridViewRow.Cells[6].Controls[0])).Text;

            //Reset the edit index.
            gridPersonData.EditIndex = -1;
            BindData();
        }

        protected void gridPerson_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable personDataTable = this.ViewState["GridPersonDataSource"] as DataTable;
            personDataTable.Rows.RemoveAt(e.RowIndex);
            personDataTable.AcceptChanges();
            this.ViewState["GridPersonDataSource"] = personDataTable;
            BindData();
        }

        private void BindData()
        {
            gridPersonData.DataSource = this.ViewState["GridPersonDataSource"];
            gridPersonData.DataBind();
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable personDataSource = this.ViewState["GridPersonDataSource"] as DataTable;
                if (personDataSource != null)
                {
                    if (personDataSource.HasItem())
                    {
                        StringBuilder failedInsertList = new StringBuilder();
                        foreach (DataRow personRow in personDataSource.Rows)
                        {
                            long? personId = personRow["Identity"].Convert<Int64>();
                            Person person = Person.GetInstance(personId);
                            person.PersonId = personId;
                            person.FirstName = personRow["FirstName"].Convert<String>();
                            person.Surname = personRow["Surname"].Convert<String>();
                            person.Age = personRow["Age"].Convert<Int32>();
                            person.Sex = personRow["Sex"].Convert<String>();
                            person.Mobile = personRow["Mobile"].Convert<String>();
                            person.Active = personRow["Active"].Convert<Boolean>();
                            bool success = person.Save();
                            if (!success)
                            {
                                failedInsertList.AppendLine(person.PersonId.ToString());
                            }
                        }
                        if (failedInsertList.Length == 0)
                        {
                            this.saveStatusLabel.Text = FileDBResource.GetResource("SavePersonFileSucceeded");
                        }
                        else
                        {
                            string message = FileDBResource.GetResource("SavePersonFilePartialySucceeded") + System.Environment.NewLine;
                            message += string.Format("Id(s): {0}", failedInsertList.ToString());
                            this.saveStatusLabel.Text = message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = FileDBResource.GetResource("SavePersonFileFailed");
                message += string.Format("Error: {0}", ex.Message);
                this.saveStatusLabel.Text = message;
            }
        }

        protected void uploadButton_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (fileUploadPerson.HasFile && fileUploadPerson.PostedFile.ContentType.Equals("text/csv"))
            {
                gridPersonData.DataSource = (DataTable)ReadToEnd(fileUploadPerson.PostedFile.FileName);
                gridPersonData.DataBind();
                this.ViewState["GridPersonDataSource"] = gridPersonData.DataSource;
                uploadStatusLabel.Text = FileDBResource.GetResource("SuccessfulDataImport");
                uploadStatusLabel.Visible = true;
                saveStatusPanel.Visible = true;
                saveButtonPanel.Visible = true;
            }
            else
            {
                uploadStatusLabel.Text = FileDBResource.GetResource("InvalidFileTypeToImport");
                uploadStatusLabel.Visible = true;
            }
        }

        private object ReadToEnd(string fileName)
        {
            string filePath = string.Format("{0}{1}", Server.MapPath("./"), fileName);
            fileUploadPerson.SaveAs(filePath);

            DataTable dtDataSource = new DataTable();

            //Read all lines from selected file and assign to string array variable.
            string[] fileContent = File.ReadAllLines(filePath);

            //Checks fileContent count > 0 then we have some lines in the file. If = 0 then file is empty
            if (fileContent.Count() > 0)
            {
                //In CSV file, 1st line contains column names. When you read CSV file, each delimited by ','.
                //fileContent[0] contains 1st line and splitted by ','. columns string array contains list of columns.
                string[] columns = fileContent[0].Split(',');
                for (int i = 0; i < columns.Count(); i++)
                {
                    dtDataSource.Columns.Add(columns[i]);
                }

                //Same logic for row data.
                for (int i = 1; i < fileContent.Count(); i++)
                {
                    string[] rowData = fileContent[i].Split(',');
                    dtDataSource.Rows.Add(rowData);
                }
            }
            return dtDataSource;
        }
    }
}