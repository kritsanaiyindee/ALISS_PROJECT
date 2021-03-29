using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.DTO
{
    public class OrganismMappingListsDTO
    {
        public Guid ogm_id { get; set; }
        public Guid ogm_mappingid { get; set; }
        public string ogm_whonetcode { get; set; }
        public string ogm_whonetdesc { get; set; }
        public string ogm_localorganismcode { get; set; }
        public string ogm_localorganismdesc { get; set; }
    }
    public class OrganismMappingSearch
    {
        public Guid ogm_mappingid { get; set; }
        public string ogm_mst_code { get; set; }
    }

 }
