using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_TCC.A.ETBUtil.CrcCalculateHelper
{
    public class CRC32
    {
        /** @brief poly: x32 + x31 + x26 + x23 + x22 + x16 + x12 + x11 + x10 + x8 + x7 + x5 + x4 + x2 + x + 1. */
        /** @brief poly: 0x84c11db7. */
        readonly static uint[] crcTableWtp =
        {
          0x00000000U, 0x84c11db7U, 0x8d4326d9U, 0x09823b6eU, 0x9e475005U, 0x1a864db2U,
            0x130476dcU, 0x97c56b6bU, 0xb84fbdbdU, 0x3c8ea00aU, 0x350c9b64U,
            0xb1cd86d3U, 0x2608edb8U, 0xa2c9f00fU, 0xab4bcb61U, 0x2f8ad6d6U,
            0xf45e66cdU, 0x709f7b7aU, 0x791d4014U, 0xfddc5da3U, 0x6a1936c8U,
            0xeed82b7fU, 0xe75a1011U, 0x639b0da6U, 0x4c11db70U, 0xc8d0c6c7U,
            0xc152fda9U, 0x4593e01eU, 0xd2568b75U, 0x569796c2U, 0x5f15adacU,
            0xdbd4b01bU, 0x6c7dd02dU, 0xe8bccd9aU, 0xe13ef6f4U, 0x65ffeb43U,
            0xf23a8028U, 0x76fb9d9fU, 0x7f79a6f1U, 0xfbb8bb46U, 0xd4326d90U,
            0x50f37027U, 0x59714b49U, 0xddb056feU, 0x4a753d95U, 0xceb42022U,
            0xc7361b4cU, 0x43f706fbU, 0x9823b6e0U, 0x1ce2ab57U, 0x15609039U,
            0x91a18d8eU, 0x0664e6e5U, 0x82a5fb52U, 0x8b27c03cU, 0x0fe6dd8bU,
            0x206c0b5dU, 0xa4ad16eaU, 0xad2f2d84U, 0x29ee3033U, 0xbe2b5b58U,
            0x3aea46efU, 0x33687d81U, 0xb7a96036U, 0xd8fba05aU, 0x5c3abdedU,
            0x55b88683U, 0xd1799b34U, 0x46bcf05fU, 0xc27dede8U, 0xcbffd686U,
            0x4f3ecb31U, 0x60b41de7U, 0xe4750050U, 0xedf73b3eU, 0x69362689U,
            0xfef34de2U, 0x7a325055U, 0x73b06b3bU, 0xf771768cU, 0x2ca5c697U,
            0xa864db20U, 0xa1e6e04eU, 0x2527fdf9U, 0xb2e29692U, 0x36238b25U,
            0x3fa1b04bU, 0xbb60adfcU, 0x94ea7b2aU, 0x102b669dU, 0x19a95df3U,
            0x9d684044U, 0x0aad2b2fU, 0x8e6c3698U, 0x87ee0df6U, 0x032f1041U,
            0xb4867077U, 0x30476dc0U, 0x39c556aeU, 0xbd044b19U, 0x2ac12072U,
            0xae003dc5U, 0xa78206abU, 0x23431b1cU, 0x0cc9cdcaU, 0x8808d07dU,
            0x818aeb13U, 0x054bf6a4U, 0x928e9dcfU, 0x164f8078U, 0x1fcdbb16U,
            0x9b0ca6a1U, 0x40d816baU, 0xc4190b0dU, 0xcd9b3063U, 0x495a2dd4U,
            0xde9f46bfU, 0x5a5e5b08U, 0x53dc6066U, 0xd71d7dd1U, 0xf897ab07U,
            0x7c56b6b0U, 0x75d48ddeU, 0xf1159069U, 0x66d0fb02U, 0xe211e6b5U,
            0xeb93dddbU, 0x6f52c06cU, 0x35365d03U, 0xb1f740b4U, 0xb8757bdaU,
            0x3cb4666dU, 0xab710d06U, 0x2fb010b1U, 0x26322bdfU, 0xa2f33668U,
            0x8d79e0beU, 0x09b8fd09U, 0x003ac667U, 0x84fbdbd0U, 0x133eb0bbU,
            0x97ffad0cU, 0x9e7d9662U, 0x1abc8bd5U, 0xc1683bceU, 0x45a92679U,
            0x4c2b1d17U, 0xc8ea00a0U, 0x5f2f6bcbU, 0xdbee767cU, 0xd26c4d12U,
            0x56ad50a5U, 0x79278673U, 0xfde69bc4U, 0xf464a0aaU, 0x70a5bd1dU,
            0xe760d676U, 0x63a1cbc1U, 0x6a23f0afU, 0xeee2ed18U, 0x594b8d2eU,
            0xdd8a9099U, 0xd408abf7U, 0x50c9b640U, 0xc70cdd2bU, 0x43cdc09cU,
            0x4a4ffbf2U, 0xce8ee645U, 0xe1043093U, 0x65c52d24U, 0x6c47164aU,
            0xe8860bfdU, 0x7f436096U, 0xfb827d21U, 0xf200464fU, 0x76c15bf8U,
            0xad15ebe3U, 0x29d4f654U, 0x2056cd3aU, 0xa497d08dU, 0x3352bbe6U,
            0xb793a651U, 0xbe119d3fU, 0x3ad08088U, 0x155a565eU, 0x919b4be9U,
            0x98197087U, 0x1cd86d30U, 0x8b1d065bU, 0x0fdc1becU, 0x065e2082U,
            0x829f3d35U, 0xedcdfd59U, 0x690ce0eeU, 0x608edb80U, 0xe44fc637U,
            0x738aad5cU, 0xf74bb0ebU, 0xfec98b85U, 0x7a089632U, 0x558240e4U,
            0xd1435d53U, 0xd8c1663dU, 0x5c007b8aU, 0xcbc510e1U, 0x4f040d56U,
            0x46863638U, 0xc2472b8fU, 0x19939b94U, 0x9d528623U, 0x94d0bd4dU,
            0x1011a0faU, 0x87d4cb91U, 0x0315d626U, 0x0a97ed48U, 0x8e56f0ffU,
            0xa1dc2629U, 0x251d3b9eU, 0x2c9f00f0U, 0xa85e1d47U, 0x3f9b762cU,
            0xbb5a6b9bU, 0xb2d850f5U, 0x36194d42U, 0x81b02d74U, 0x057130c3U,
            0x0cf30badU, 0x8832161aU, 0x1ff77d71U, 0x9b3660c6U, 0x92b45ba8U,
            0x1675461fU, 0x39ff90c9U, 0xbd3e8d7eU, 0xb4bcb610U, 0x307daba7U,
            0xa7b8c0ccU, 0x2379dd7bU, 0x2afbe615U, 0xae3afba2U, 0x75ee4bb9U,
            0xf12f560eU, 0xf8ad6d60U, 0x7c6c70d7U, 0xeba91bbcU, 0x6f68060bU,
            0x66ea3d65U, 0xe22b20d2U, 0xcda1f604U, 0x4960ebb3U, 0x40e2d0ddU,
            0xc423cd6aU, 0x53e6a601U, 0xd727bbb6U, 0xdea580d8U, 0x5a649d6fU,
        };

        /** @brief poly: x32 + x26 + x23 + x22 + x16 + x12 + x11 + x10 + x8 + x7 + x5 + x4 + x2 + x + 1.
        poly: 0x4c11db7. */
        readonly static uint[] crcTableEthernet =
        {
          0x00000000, 0x04c11db7, 0x09823b6e, 0x0d4326d9, 0x130476dc,
            0x17c56b6b, 0x1a864db2, 0x1e475005, 0x2608edb8, 0x22c9f00f,
            0x2f8ad6d6, 0x2b4bcb61, 0x350c9b64, 0x31cd86d3, 0x3c8ea00a,
            0x384fbdbd, 0x4c11db70, 0x48d0c6c7, 0x4593e01e, 0x4152fda9,
            0x5f15adac, 0x5bd4b01b, 0x569796c2, 0x52568b75, 0x6a1936c8,
            0x6ed82b7f, 0x639b0da6, 0x675a1011, 0x791d4014, 0x7ddc5da3,
            0x709f7b7a, 0x745e66cd, 0x9823b6e0, 0x9ce2ab57, 0x91a18d8e,
            0x95609039, 0x8b27c03c, 0x8fe6dd8b, 0x82a5fb52, 0x8664e6e5,
            0xbe2b5b58, 0xbaea46ef, 0xb7a96036, 0xb3687d81, 0xad2f2d84,
            0xa9ee3033, 0xa4ad16ea, 0xa06c0b5d, 0xd4326d90, 0xd0f37027,
            0xddb056fe, 0xd9714b49, 0xc7361b4c, 0xc3f706fb, 0xceb42022,
            0xca753d95, 0xf23a8028, 0xf6fb9d9f, 0xfbb8bb46, 0xff79a6f1,
            0xe13ef6f4, 0xe5ffeb43, 0xe8bccd9a, 0xec7dd02d, 0x34867077,
            0x30476dc0, 0x3d044b19, 0x39c556ae, 0x278206ab, 0x23431b1c,
            0x2e003dc5, 0x2ac12072, 0x128e9dcf, 0x164f8078, 0x1b0ca6a1,
            0x1fcdbb16, 0x018aeb13, 0x054bf6a4, 0x0808d07d, 0x0cc9cdca,
            0x7897ab07, 0x7c56b6b0, 0x71159069, 0x75d48dde, 0x6b93dddb,
            0x6f52c06c, 0x6211e6b5, 0x66d0fb02, 0x5e9f46bf, 0x5a5e5b08,
            0x571d7dd1, 0x53dc6066, 0x4d9b3063, 0x495a2dd4, 0x44190b0d,
            0x40d816ba, 0xaca5c697, 0xa864db20, 0xa527fdf9, 0xa1e6e04e,
            0xbfa1b04b, 0xbb60adfc, 0xb6238b25, 0xb2e29692, 0x8aad2b2f,
            0x8e6c3698, 0x832f1041, 0x87ee0df6, 0x99a95df3, 0x9d684044,
            0x902b669d, 0x94ea7b2a, 0xe0b41de7, 0xe4750050, 0xe9362689,
            0xedf73b3e, 0xf3b06b3b, 0xf771768c, 0xfa325055, 0xfef34de2,
            0xc6bcf05f, 0xc27dede8, 0xcf3ecb31, 0xcbffd686, 0xd5b88683,
            0xd1799b34, 0xdc3abded, 0xd8fba05a, 0x690ce0ee, 0x6dcdfd59,
            0x608edb80, 0x644fc637, 0x7a089632, 0x7ec98b85, 0x738aad5c,
            0x774bb0eb, 0x4f040d56, 0x4bc510e1, 0x46863638, 0x42472b8f,
            0x5c007b8a, 0x58c1663d, 0x558240e4, 0x51435d53, 0x251d3b9e,
            0x21dc2629, 0x2c9f00f0, 0x285e1d47, 0x36194d42, 0x32d850f5,
            0x3f9b762c, 0x3b5a6b9b, 0x0315d626, 0x07d4cb91, 0x0a97ed48,
            0x0e56f0ff, 0x1011a0fa, 0x14d0bd4d, 0x19939b94, 0x1d528623,
            0xf12f560e, 0xf5ee4bb9, 0xf8ad6d60, 0xfc6c70d7, 0xe22b20d2,
            0xe6ea3d65, 0xeba91bbc, 0xef68060b, 0xd727bbb6, 0xd3e6a601,
            0xdea580d8, 0xda649d6f, 0xc423cd6a, 0xc0e2d0dd, 0xcda1f604,
            0xc960ebb3, 0xbd3e8d7e, 0xb9ff90c9, 0xb4bcb610, 0xb07daba7,
            0xae3afba2, 0xaafbe615, 0xa7b8c0cc, 0xa379dd7b, 0x9b3660c6,
            0x9ff77d71, 0x92b45ba8, 0x9675461f, 0x8832161a, 0x8cf30bad,
            0x81b02d74, 0x857130c3, 0x5d8a9099, 0x594b8d2e, 0x5408abf7,
            0x50c9b640, 0x4e8ee645, 0x4a4ffbf2, 0x470cdd2b, 0x43cdc09c,
            0x7b827d21, 0x7f436096, 0x7200464f, 0x76c15bf8, 0x68860bfd,
            0x6c47164a, 0x61043093, 0x65c52d24, 0x119b4be9, 0x155a565e,
            0x18197087, 0x1cd86d30, 0x029f3d35, 0x065e2082, 0x0b1d065b,
            0x0fdc1bec, 0x3793a651, 0x3352bbe6, 0x3e119d3f, 0x3ad08088,
            0x2497d08d, 0x2056cd3a, 0x2d15ebe3, 0x29d4f654, 0xc5a92679,
            0xc1683bce, 0xcc2b1d17, 0xc8ea00a0, 0xd6ad50a5, 0xd26c4d12,
            0xdf2f6bcb, 0xdbee767c, 0xe3a1cbc1, 0xe760d676, 0xea23f0af,
            0xeee2ed18, 0xf0a5bd1d, 0xf464a0aa, 0xf9278673, 0xfde69bc4,
            0x89b8fd09, 0x8d79e0be, 0x803ac667, 0x84fbdbd0, 0x9abc8bd5,
            0x9e7d9662, 0x933eb0bb, 0x97ffad0c, 0xafb010b1, 0xab710d06,
            0xa6322bdf, 0xa2f33668, 0xbcb4666d, 0xb8757bda, 0xb5365d03,
            0xb1f740b4
        };

        /// <summary>
        /// CRC-IO Table
        /// </summary>
        private static UInt32[] ulCbiPlatformCRC32Table =
        {
            0x00000000U, 0x77073096U, 0xee0e612cU, 0x990951baU, 0x076dc419U, 0x706af48fU, 0xe963a535U, 0x9e6495a3U,
            0x0edb8832U, 0x79dcb8a4U, 0xe0d5e91eU, 0x97d2d988U, 0x09b64c2bU, 0x7eb17cbdU, 0xe7b82d07U, 0x90bf1d91U,
            0x1db71064U, 0x6ab020f2U, 0xf3b97148U, 0x84be41deU, 0x1adad47dU, 0x6ddde4ebU, 0xf4d4b551U, 0x83d385c7U,
            0x136c9856U, 0x646ba8c0U, 0xfd62f97aU, 0x8a65c9ecU, 0x14015c4fU, 0x63066cd9U, 0xfa0f3d63U, 0x8d080df5U,
            0x3b6e20c8U, 0x4c69105eU, 0xd56041e4U, 0xa2677172U, 0x3c03e4d1U, 0x4b04d447U, 0xd20d85fdU, 0xa50ab56bU,
            0x35b5a8faU, 0x42b2986cU, 0xdbbbc9d6U, 0xacbcf940U, 0x32d86ce3U, 0x45df5c75U, 0xdcd60dcfU, 0xabd13d59U,
            0x26d930acU, 0x51de003aU, 0xc8d75180U, 0xbfd06116U, 0x21b4f4b5U, 0x56b3c423U, 0xcfba9599U, 0xb8bda50fU,
            0x2802b89eU, 0x5f058808U, 0xc60cd9b2U, 0xb10be924U, 0x2f6f7c87U, 0x58684c11U, 0xc1611dabU, 0xb6662d3dU,
            0x76dc4190U, 0x01db7106U, 0x98d220bcU, 0xefd5102aU, 0x71b18589U, 0x06b6b51fU, 0x9fbfe4a5U, 0xe8b8d433U,
            0x7807c9a2U, 0x0f00f934U, 0x9609a88eU, 0xe10e9818U, 0x7f6a0dbbU, 0x086d3d2dU, 0x91646c97U, 0xe6635c01U,
            0x6b6b51f4U, 0x1c6c6162U, 0x856530d8U, 0xf262004eU, 0x6c0695edU, 0x1b01a57bU, 0x8208f4c1U, 0xf50fc457U,
            0x65b0d9c6U, 0x12b7e950U, 0x8bbeb8eaU, 0xfcb9887cU, 0x62dd1ddfU, 0x15da2d49U, 0x8cd37cf3U, 0xfbd44c65U,
            0x4db26158U, 0x3ab551ceU, 0xa3bc0074U, 0xd4bb30e2U, 0x4adfa541U, 0x3dd895d7U, 0xa4d1c46dU, 0xd3d6f4fbU,
            0x4369e96aU, 0x346ed9fcU, 0xad678846U, 0xda60b8d0U, 0x44042d73U, 0x33031de5U, 0xaa0a4c5fU, 0xdd0d7cc9U,
            0x5005713cU, 0x270241aaU, 0xbe0b1010U, 0xc90c2086U, 0x5768b525U, 0x206f85b3U, 0xb966d409U, 0xce61e49fU,
            0x5edef90eU, 0x29d9c998U, 0xb0d09822U, 0xc7d7a8b4U, 0x59b33d17U, 0x2eb40d81U, 0xb7bd5c3bU, 0xc0ba6cadU,
            0xedb88320U, 0x9abfb3b6U, 0x03b6e20cU, 0x74b1d29aU, 0xead54739U, 0x9dd277afU, 0x04db2615U, 0x73dc1683U,
            0xe3630b12U, 0x94643b84U, 0x0d6d6a3eU, 0x7a6a5aa8U, 0xe40ecf0bU, 0x9309ff9dU, 0x0a00ae27U, 0x7d079eb1U,
            0xf00f9344U, 0x8708a3d2U, 0x1e01f268U, 0x6906c2feU, 0xf762575dU, 0x806567cbU, 0x196c3671U, 0x6e6b06e7U,
            0xfed41b76U, 0x89d32be0U, 0x10da7a5aU, 0x67dd4accU, 0xf9b9df6fU, 0x8ebeeff9U, 0x17b7be43U, 0x60b08ed5U,
            0xd6d6a3e8U, 0xa1d1937eU, 0x38d8c2c4U, 0x4fdff252U, 0xd1bb67f1U, 0xa6bc5767U, 0x3fb506ddU, 0x48b2364bU,
            0xd80d2bdaU, 0xaf0a1b4cU, 0x36034af6U, 0x41047a60U, 0xdf60efc3U, 0xa867df55U, 0x316e8eefU, 0x4669be79U,
            0xcb61b38cU, 0xbc66831aU, 0x256fd2a0U, 0x5268e236U, 0xcc0c7795U, 0xbb0b4703U, 0x220216b9U, 0x5505262fU,
            0xc5ba3bbeU, 0xb2bd0b28U, 0x2bb45a92U, 0x5cb36a04U, 0xc2d7ffa7U, 0xb5d0cf31U, 0x2cd99e8bU, 0x5bdeae1dU,
            0x9b64c2b0U, 0xec63f226U, 0x756aa39cU, 0x026d930aU, 0x9c0906a9U, 0xeb0e363fU, 0x72076785U, 0x05005713U,
            0x95bf4a82U, 0xe2b87a14U, 0x7bb12baeU, 0x0cb61b38U, 0x92d28e9bU, 0xe5d5be0dU, 0x7cdcefb7U, 0x0bdbdf21U,
            0x86d3d2d4U, 0xf1d4e242U, 0x68ddb3f8U, 0x1fda836eU, 0x81be16cdU, 0xf6b9265bU, 0x6fb077e1U, 0x18b74777U,
            0x88085ae6U, 0xff0f6a70U, 0x66063bcaU, 0x11010b5cU, 0x8f659effU, 0xf862ae69U, 0x616bffd3U, 0x166ccf45U,
            0xa00ae278U, 0xd70dd2eeU, 0x4e048354U, 0x3903b3c2U, 0xa7672661U, 0xd06016f7U, 0x4969474dU, 0x3e6e77dbU,
            0xaed16a4aU, 0xd9d65adcU, 0x40df0b66U, 0x37d83bf0U, 0xa9bcae53U, 0xdebb9ec5U, 0x47b2cf7fU, 0x30b5ffe9U,
            0xbdbdf21cU, 0xcabac28aU, 0x53b39330U, 0x24b4a3a6U, 0xbad03605U, 0xcdd70693U, 0x54de5729U, 0x23d967bfU,
            0xb3667a2eU, 0xc4614ab8U, 0x5d681b02U, 0x2a6f2b94U, 0xb40bbe37U, 0xc30c8ea1U, 0x5a05df1bU, 0x2d02ef8dU
        };


        #region 计算Crc函数

        #region CACT.ETB

        /// <summary>
        /// Calculate WTP Crc Table
        /// </summary>
        /// <param name="bufferArr">Buffter Start Address</param>
        /// <param name="ulSize">The size of Calculate Buffter </param>
        /// <returns></returns>
        public static UInt32 CalculateWtpCrc(List<Byte> bufferArr, int ulSize)
        {
            if (bufferArr == null)
            {
                throw new ArgumentNullException("参数bufferArr不能为空。");
            }
            if (ulSize < 0)
            {
                throw new ArgumentException("参数ulSize不能小于0。");
            }
            if (bufferArr.Count < ulSize)
            {
                throw new ArgumentException("参数ulSize不能大于参数bufferArr中的数据的个数。");
            }
            int ulI;
            var ulAccum = 0U;
            for (ulI = 0; ulI < ulSize; ulI++)
            {
                ulAccum = (ulAccum << 8) ^ crcTableWtp[(Byte)(((ulAccum >> 24) ^ bufferArr[ulI]) & 0xFFU)];
            }
            return ulAccum;
        }

        /// <summary>
        /// Calculate Ethernet Crc Table
        /// </summary>
        /// <param name="bufferArr">Buffter Start Address</param>
        /// <param name="ulSize">The size of Calculate Buffter </param>
        /// <returns>Value</returns>
        public static UInt32 CalculateEthnetCrc(List<Byte> bufferArr, int ulSize)
        {
            if (bufferArr == null)
            {
                throw new ArgumentNullException("参数bufferArr不能为空。");
            }
            if (ulSize < 0)
            {
                throw new ArgumentException("参数ulSize不能小于0。");
            }
            if (bufferArr.Count < ulSize)
            {
                throw new ArgumentException("参数ulSize不能大于参数bufferArr中的数据的个数。");
            }
            int ulI;               /* Loop index */
            var ulAccum = 0U;  /* Accumulate sum */
            for (ulI = 0; ulI < ulSize; ulI++)
            {
                ulAccum = (ulAccum << 8) ^ crcTableEthernet[(Byte)(((ulAccum >> 24) ^ bufferArr[ulI]) & 0xFFU)];
            }
            return ulAccum;
        }

        /// <summary>
        /// Calculate Platform Wtp Crc Table
        /// </summary>
        /// <param name="pucData">Buffter</param>
        /// <param name="ulSize">The size of Calculate Buffter</param>
        /// <returns>Value</returns>
        public static UInt32 CalculatePlatformWtpCrc(List<Byte> pucData, int ulSize)
        {
            if (pucData == null)
            {
                throw new ArgumentNullException("参数pucData不能为空。");
            }
            if (ulSize < 0)
            {
                throw new ArgumentException("参数ulSize不能小于0。");
            }
            if (pucData.Count < ulSize)
            {
                throw new ArgumentException("参数ulSize不能大于参数pucData中的数据的个数。");
            }
            int ulLoop;               /* Loop index */
            var ulAccum = 0xFFFFFFFF;  /* Accumulate sum */

            for (ulLoop = 0; ulLoop < ulSize; ulLoop++)
            {
                ulAccum = (ulAccum >> 8) ^ ulCbiPlatformCRC32Table[(Byte)(ulAccum & 0xFFU) ^ pucData[ulLoop]];
            }

            ulAccum ^= 0xFFFFFFFFU;
            return ulAccum;
        }

        #endregion
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf">待校验数据</param>
        /// <param name="off">待校验数据起始索引，从0开始</param>
        /// <param name="len">待校验数据长度</param>
        /// <param name="checkSum">多项式类型</param>
        /// <returns></returns>
        public static uint GetCheckSum(byte[] buf, int off, int len, POLYNOMIAL polynomial)
        {
            var crcTable = POLYNOMIAL.ETHERNET == polynomial ? crcTableEthernet : crcTableWtp;

            uint c = 0x00;
            while (--len >= 0)
                c = crcTable[((c >> 24) ^ buf[off++]) & 0xff] ^ ((c << 8));
            return c;
        }

        /// <summary>
        /// 从字符串的@off位置开始校验@length个字符，每两个字符为一个byte
        /// </summary>
        /// <param name="str">待校验的十六进制字符串</param>
        /// <param name="length">待校验的十六进制数</param>
        /// <param name="crctype">多项式类型</param>
        /// <param name="off">待校验的字符串的起始位置，从0开始</param>
        /// <returns></returns>
        public static uint GetCheckSum(string str, int length, POLYNOMIAL polynomial, int off = 0)
        {
            uint checkSum = 0;

            if (length * 2 > str.Length)
            {
                return checkSum;
            }


            uint tableIndex = 0;
            var crcTable = POLYNOMIAL.ETHERNET == polynomial ? crcTableEthernet : crcTableWtp;

            for (int l_index = off; l_index < length * 2; )
            {
                int p = str.Length;
                string substr = str.Substring(l_index, 2);
                byte var = byte.Parse(substr, System.Globalization.NumberStyles.HexNumber);

                tableIndex = ((checkSum >> 24) ^ var) & 0xFFU;
                checkSum = (checkSum << 8) ^ crcTable[tableIndex];
                l_index += 2;
            }

            return checkSum;
        }

        #endregion 计算Crc函数
    }

    public enum POLYNOMIAL : byte
    {
        WTP,
        ETHERNET
    }
}
