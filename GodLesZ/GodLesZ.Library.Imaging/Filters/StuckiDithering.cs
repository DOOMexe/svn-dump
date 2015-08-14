﻿namespace GodLesZ.Library.Imaging.Filters {

	public class StuckiDithering : ErrorDiffusionDithering
    {
        private static int[] coef1 = new int[] { 2, 4, 8, 4, 2 };
        private static int[] coef2 = new int[] { 1, 2, 4, 2, 1 };

        protected override unsafe void Diffuse(int error, byte* ptr)
        {
            int num;
            if (base.x < base.widthM1)
            {
                num = ptr[1] + ((error * 8) / 0x2a);
                num = (num < 0) ? 0 : ((num > 0xff) ? 0xff : num);
                ptr[1] = (byte) num;
            }
            if (base.x < (base.widthM1 - 1))
            {
                num = ptr[2] + ((error * 4) / 0x2a);
                num = (num < 0) ? 0 : ((num > 0xff) ? 0xff : num);
                ptr[2] = (byte) num;
            }
            if (base.y < base.heightM1)
            {
                ptr += base.stride;
                int index = -2;
                for (int i = 0; index <= 2; i++)
                {
                    if (((base.x + index) >= 0) && ((base.x + index) < base.width))
                    {
                        num = ptr[index] + ((error * coef1[i]) / 0x2a);
                        num = (num < 0) ? 0 : ((num > 0xff) ? 0xff : num);
                        ptr[index] = (byte) num;
                    }
                    index++;
                }
            }
            if (base.y < (base.heightM1 - 1))
            {
                ptr += base.stride;
                int num4 = -2;
                for (int j = 0; num4 <= 2; j++)
                {
                    if (((base.x + num4) >= 0) && ((base.x + num4) < base.width))
                    {
                        num = ptr[num4] + ((error * coef2[j]) / 0x2a);
                        num = (num < 0) ? 0 : ((num > 0xff) ? 0xff : num);
                        ptr[num4] = (byte) num;
                    }
                    num4++;
                }
            }
        }
    }
}

