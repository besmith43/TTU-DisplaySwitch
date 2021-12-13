HRESULT hr = S_OK;
    UINT32 NumPathArrayElements = 0;
    UINT32 NumModeInfoArrayElements = 0;
    //LONG error = GetDisplayConfigBufferSizes((QDC_ALL_PATHS | QDC_VIRTUAL_MODE_AWARE), &NumPathArrayElements, &NumModeInfoArrayElements);
    hr = GetDisplayConfigBufferSizes((QDC_ALL_PATHS), &NumPathArrayElements, &NumModeInfoArrayElements);
    std::vector<DISPLAYCONFIG_PATH_INFO> PathInfoArray2(NumPathArrayElements);
    std::vector<DISPLAYCONFIG_MODE_INFO> ModeInfoArray2(NumModeInfoArrayElements);
    //error = QueryDisplayConfig((QDC_ALL_PATHS | QDC_VIRTUAL_MODE_AWARE), &NumPathArrayElements, &PathInfoArray2[0], &NumModeInfoArrayElements, &ModeInfoArray2[0], NULL);
    hr = QueryDisplayConfig((QDC_ALL_PATHS), &NumPathArrayElements, &PathInfoArray2[0], &NumModeInfoArrayElements, &ModeInfoArray2[0], NULL);

    struct displaySourcePair
    {
        std::wstring displayName;
        UINT32 displayId;
    };

    std::vector<displaySourcePair> ocupiedDisplays;

    if (hr == S_OK)
    {

        DISPLAYCONFIG_SOURCE_DEVICE_NAME SourceName = {};
        SourceName.header.type = DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME;
        SourceName.header.size = sizeof(SourceName);

        DISPLAYCONFIG_TARGET_PREFERRED_MODE PreferedMode = {};
        PreferedMode.header.type = DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_PREFERRED_MODE;
        PreferedMode.header.size = sizeof(PreferedMode);


        int newId = 0;


        for (UINT32 i = 0; i < NumPathArrayElements; i++)
        {
            bool match = false;
            SourceName.header.adapterId = PathInfoArray2[i].sourceInfo.adapterId;
            SourceName.header.id = PathInfoArray2[i].sourceInfo.id;

            PreferedMode.header.adapterId = PathInfoArray2[i].targetInfo.adapterId;
            PreferedMode.header.id = PathInfoArray2[i].targetInfo.id;

            hr = HRESULT_FROM_WIN32(DisplayConfigGetDeviceInfo(&SourceName.header));
            hr = HRESULT_FROM_WIN32(DisplayConfigGetDeviceInfo(&PreferedMode.header));

            if (hr == S_OK)
            {

                if ((PathInfoArray2[i].flags & DISPLAYCONFIG_PATH_ACTIVE) == true)
                {
                    std::wstring str = std::wstring(SourceName.viewGdiDeviceName);
                    displaySourcePair tmpStruct;
                    tmpStruct.displayId = PreferedMode.header.id;
                    tmpStruct.displayName = str;
                    ocupiedDisplays.push_back(tmpStruct);
                }

                for (int k = 0; k < ocupiedDisplays.size(); k++)
                {
                    std::wstring str = std::wstring(SourceName.viewGdiDeviceName);
                    if (ocupiedDisplays[k].displayName == str || ocupiedDisplays[k].displayId == PreferedMode.header.id)
                    {
                        match = true;
                    }
                }

                if (match == false && PathInfoArray2[i].targetInfo.targetAvailable == 1)
                {
                    PathInfoArray2[i].flags |= DISPLAYCONFIG_PATH_ACTIVE;
                    std::wstring str = std::wstring(SourceName.viewGdiDeviceName);
                    displaySourcePair tmpStruct;
                    tmpStruct.displayId = PreferedMode.header.id;
                    tmpStruct.displayName = str;
                    ocupiedDisplays.push_back(tmpStruct);
                }

                if (PathInfoArray2[i].targetInfo.targetAvailable == 1)
                {
                    PathInfoArray2[i].sourceInfo.id = newId;
                    newId++;
                }

                if (PathInfoArray2[i].targetInfo.id != PreferedMode.header.id)
                {
                    PathInfoArray2[i].targetInfo.id = PreferedMode.header.id;
                }

                PathInfoArray2[i].sourceInfo.modeInfoIdx = DISPLAYCONFIG_PATH_MODE_IDX_INVALID;
                PathInfoArray2[i].targetInfo.modeInfoIdx = DISPLAYCONFIG_PATH_MODE_IDX_INVALID;
            }
        }

        //hr = SetDisplayConfig(NumPathArrayElements, &PathInfoArray2[0], 0, NULL, (SDC_VALIDATE | SDC_TOPOLOGY_SUPPLIED | SDC_ALLOW_PATH_ORDER_CHANGES | SDC_VIRTUAL_MODE_AWARE));
        //hr = SetDisplayConfig(NumPathArrayElements, &PathInfoArray2[0], 0, NULL, (SDC_APPLY | SDC_TOPOLOGY_SUPPLIED | SDC_ALLOW_PATH_ORDER_CHANGES | SDC_VIRTUAL_MODE_AWARE));
        hr = SetDisplayConfig(NumPathArrayElements, &PathInfoArray2[0], 0, NULL, (SDC_VALIDATE | SDC_TOPOLOGY_SUPPLIED | SDC_ALLOW_PATH_ORDER_CHANGES));
        hr = SetDisplayConfig(NumPathArrayElements, &PathInfoArray2[0], 0, NULL, (SDC_APPLY | SDC_TOPOLOGY_SUPPLIED | SDC_ALLOW_PATH_ORDER_CHANGES));
    }