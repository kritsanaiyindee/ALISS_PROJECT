using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ALISS.Mapping.DTO;
using ExcelDataReader;
using System.Text;
using DbfDataReader;
using Microsoft.AspNetCore.Hosting;
using System.Data;

namespace ALISS.Data.D2_Mapping
{
    public class TemplateUploadService
    {
        private readonly IWebHostEnvironment _environment;
        public TemplateUploadService(IWebHostEnvironment env)
        {
            _environment = env;
        }
        public async Task<List<TemplateFileListsDTO>> UploadAsync(IFileListEntry fileEntry,bool? FirstLineIsHeader)
        {
            var Templates = new List<TemplateFileListsDTO>();
            string _tempheader = "", _tempvalue = "",path = "";
            int _temprow = 0;
            try
            {
                var g = Path.GetExtension(fileEntry.Name);
                #region ReadExcel
                if (Path.GetExtension(fileEntry.Name) == ".xls" || Path.GetExtension(fileEntry.Name) == ".xlsx")
                {
                    using (var stream = new MemoryStream())
                    {
                        await fileEntry.Data.CopyToAsync(stream);
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            //var result = reader.AsDataSet();
                            do
                            {
                                _temprow = 1;

                                while (reader.Read() && _temprow <= 2) //Each ROW
                                {
                                    if (_temprow == 1)
                                    {
                                        for (int column = 0; column < reader.FieldCount; column++)
                                        {
                                            if (FirstLineIsHeader == true)
                                            {
                                                if (reader.GetValue(column) != null)
                                                {
                                                    _tempheader = reader.GetValue(column).ToString().Trim();
                                                }
                                                else
                                                    _tempheader = "";
                                            }
                                            else
                                            {
                                                _tempheader = "Column" + (column + 1).ToString();

                                                if (reader.GetValue(column) != null)
                                                {
                                                    _tempvalue = reader.GetValue(column).ToString();
                                                }
                                                else
                                                    _tempvalue = "";
                                            }

                                            Templates.Add(new TemplateFileListsDTO
                                            {
                                                tmp_no = column,
                                                tmp_header = _tempheader,
                                                tmp_value = _tempvalue
                                            }
                                            );
                                        }
                                    }
                                    else if (_temprow == 2 && FirstLineIsHeader == true)
                                    {
                                        for (int column = 0; column < reader.FieldCount; column++)
                                        {
                                            TemplateFileListsDTO template = Templates.Where(t => t.tmp_no == column).FirstOrDefault();
                                            if (reader.GetValue(column) != null)
                                            {
                                                var fieldtype = reader.GetFieldType(column).ToString();
                                                string temp_value = "";

                                                if (fieldtype == "System.DateTime")
                                                {
                                                    temp_value = reader.GetDateTime(column).ToString();
                                                }
                                                else
                                                {
                                                    temp_value = reader.GetValue(column).ToString();
                                                }
                                                template.tmp_value = temp_value;
                                                //var testvalue = reader.GetValue(column);
                                            }
                                        }
                                    }
                                    _temprow++;

                                }
                            } while (reader.NextResult());

                        }
                    }
                }
                #endregion
                #region Readcsv
                    else if(Path.GetExtension(fileEntry.Name) == ".csv")
                    {

                    using (var stream = new MemoryStream())
                    {
                        await fileEntry.Data.CopyToAsync(stream);
                        var reader = ExcelReaderFactory.CreateCsvReader(stream, new ExcelReaderConfiguration()
                        {
                            FallbackEncoding = Encoding.GetEncoding(1252),
                            AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },
                            LeaveOpen = false,
                            AnalyzeInitialCsvRows = 0,
                        });

                        var result = reader.AsDataSet();
                        do
                        {
                            _temprow = 1;

                            while (reader.Read() && _temprow <= 2) //Each ROW
                            {
                                if (_temprow == 1)
                                {
                                    for (int column = 0; column < reader.FieldCount; column++)
                                    {
                                        if (FirstLineIsHeader == true)
                                        {
                                            if (reader.GetValue(column) != null)
                                            {
                                                _tempheader = reader.GetValue(column).ToString().Trim();
                                            }
                                            else
                                                _tempheader = "";
                                        }
                                        else
                                        {
                                            _tempheader = "Column" + (column + 1).ToString();

                                            if (reader.GetValue(column) != null)
                                            {
                                                _tempvalue = reader.GetValue(column).ToString();
                                            }
                                            else
                                                _tempvalue = "";
                                        }

                                        Templates.Add(new TemplateFileListsDTO
                                        {
                                            tmp_no = column,
                                            tmp_header = _tempheader,
                                            tmp_value = _tempvalue
                                        }
                                        );
                                    }
                                }
                                else if (_temprow == 2 && FirstLineIsHeader == true)
                                {
                                    for (int column = 0; column < reader.FieldCount; column++)
                                    {
                                        TemplateFileListsDTO template = Templates.Where(t => t.tmp_no == column).FirstOrDefault();
                                        if (reader.GetValue(column) != null)
                                        {
                                            template.tmp_value = reader.GetValue(column).ToString();
                                            var testvalue = reader.GetValue(column);
                                        }
                                    }
                                }
                                _temprow++;

                            }
                        } while (reader.NextResult());
                    }


                    }
                #endregion
                #region ReadText
                else if (Path.GetExtension(fileEntry.Name) == ".txt")
                    {
                  
                    string tempFilename = Guid.NewGuid().ToString() + ".txt";
                    path = Path.Combine(_environment.ContentRootPath, "TempFile", tempFilename);
                    var ms = new MemoryStream();
                    await fileEntry.Data.CopyToAsync(ms);
                    using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        ms.WriteTo(file);
                    }

                    DataTable dt = new DataTable();

                    using (TextReader tr = File.OpenText("TempFile/" + tempFilename))
                    {
                        string line;
                        _temprow = 1;
                        while ((line = tr.ReadLine()) != null && _temprow <=2)
                        {
                            string[] items = line.Split('\t');
                           

                                if (_temprow == 1)
                                {
                                    for (int i = 0; i < items.Length; i++)
                                    {
                                        if (FirstLineIsHeader == true)
                                        {
                                            _tempheader = items[i].ToString();
                                        }
                                        else
                                        {
                                            _tempheader = "Column" + (i + 1).ToString();
                                            _tempvalue = items[i].ToString();
                                    }
                                        Templates.Add(new TemplateFileListsDTO
                                        {
                                            tmp_no = i,
                                            tmp_header = _tempheader,
                                            tmp_value = _tempvalue
                                        }
                                        );
                                    }
                                }
                                else if (_temprow == 2 && FirstLineIsHeader == true)
                                {
                                    for (int i = 0; i < items.Length; i++)
                                    {
                                    TemplateFileListsDTO template = Templates.Where(t => t.tmp_no == i).FirstOrDefault();
                                    template.tmp_value = items[i].ToString();
                                    }
                                }                            



                                _temprow++;
                      
                        }
                    }
                 

                    File.Delete(path);

                }
            
                    #endregion
                else
                    {
                        string tempFilename = Guid.NewGuid().ToString() + ".dbf";
                    

                    path = Path.Combine(_environment.ContentRootPath, "TempFile", tempFilename);

                    bool exists = System.IO.Directory.Exists(Path.Combine(_environment.ContentRootPath, "TempFile"));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, "TempFile"));

                    using (FileStream file = new FileStream(path, FileMode.Create))                    
                    {
                        try{
                            await fileEntry.Data.CopyToAsync(file);
                        }
                        catch(Exception ex) 
                        { 

                        }                         
                        finally { 
                            file.Flush(); 
                        }
                        
                       
                                         
                    }

                    //using (var dbfTable = new DbfTable("TempFile/" + tempFilename, Encoding.GetEncoding(1252)))
                    using (var dbfTable = new DbfTable("TempFile/" + tempFilename, Encoding.GetEncoding(874)))
                    {
                        var header = dbfTable.Header;
                        var recordCount = header.RecordCount;
                        var column = 1;

                        if (FirstLineIsHeader == true)
                        {
                            foreach (var dbfColumn in dbfTable.Columns)
                            {
                                //var name = dbfColumn.Name;
                                //var columnType = dbfColumn.ColumnType;
                                //var length = dbfColumn.Length;
                                //var decimalCount = dbfColumn.DecimalCount;

                                Templates.Add(new TemplateFileListsDTO
                                {
                                    tmp_no = column,
                                    tmp_header = dbfColumn.Name
                                }
                                );
                                column++;
                            }

                        }
                        else
                        {
                            foreach (var dbfColumn in dbfTable.Columns)
                            {
                                Templates.Add(new TemplateFileListsDTO
                                {
                                    tmp_no = column,
                                    tmp_header = "Column" + (column).ToString()
                                }
                                );
                                column++;
                            }
                        }

                        column = 1;
                        _temprow = 1;
                        var dbfRecord = new DbfRecord(dbfTable);
                        while (dbfTable.Read(dbfRecord) && _temprow < 2)
                        {
                            foreach (var dbfValue in dbfRecord.Values)
                            {
                                TemplateFileListsDTO template = Templates.Where(t => t.tmp_no == column).FirstOrDefault();
                                if (dbfValue != null)
                                {
                                    template.tmp_value = dbfValue.ToString();
                                }
                                //var stringValue = dbfValue.ToString();
                                //var obj = dbfValue.GetValue();
                                column++;
                            }
                            _temprow++;
                        }


                    }
                    File.Delete(path);
                }
            }
            catch(Exception ex)
            {
                return Templates;
            }
            return Templates;
        }

    }
}
