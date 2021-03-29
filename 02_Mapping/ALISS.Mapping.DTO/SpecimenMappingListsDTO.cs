using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.DTO
{
    public class SpecimenMappingListsDTO
    {
        public Guid spm_id { get; set; }
        public Guid spm_mappingid { get; set; }
        public string spm_whonetcode { get; set; }
        public string spm_localspecimencode { get; set; }
        public string spm_localspecimendesc { get; set; }
    }

    public class SpecimenMappingSearch
    {
        public Guid spm_mappingid { get; set; }       
        public string spm_mst_code { get; set; }
    }
}
