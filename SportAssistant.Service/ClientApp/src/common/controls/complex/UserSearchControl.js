import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync } from "../../ApiActions";
import { InputNumber, InputText, Tooltip } from "../CustomControls";

// handleSearchResult - обработка карточки юзера, которую нашли, и ошибок. handleSearchResult(userCard, errorText);
// showIdSerach -  показывать ли поиск по Ид.
export function UserSearchControl({ handleSearchResult, userName = '', showIdSerach = true }) {
  const { t } = useTranslation(); //language pack implementation
  const [userSearch, searchChange] = useState({ id: 0, login: '' });
  const [showSearchBlock, onEdit] = useState(!userName);

  return (
    <Row>
      {!showSearchBlock ?
        <Container fluid>
          {userName}
          <img id="searchUserImg" src="/img/edit_icon.png" width="25" height="25" className="rounded mx-auto spaceLeft clickable" alt="" onClick={() => onEdit(true)} />
          <Tooltip text={t('general.actions.enableSearch')} tooltipTargetId="searchUserImg" />
        </Container>
        :
        <>
          {showSearchBlock && userName !== '' &&
            <Col xs={1} style={{ marginRight: '-70px', marginTop: '5px' }}>
              <img id="cancelSearchImg" src="/img/close_icon.png" width="25" height="25" className="rounded mx-auto spaceRight clickable" alt="" onClick={() => onEdit(false)} />
              <Tooltip text={t('general.actions.cancelSearch')} tooltipTargetId="cancelSearchImg" />
            </Col>
          }
          <Col xs={3}>
            <InputText label={t('appSetup.admin.byLogin')} initialValue={userSearch.login} propName="login" onChange={onSearchChange} />
          </Col>

          {showIdSerach &&
            <Col xs={3}>
              <InputNumber label={t('appSetup.admin.byId')} initialValue={userSearch.id} propName="id" onChange={onSearchChange} />
            </Col>
          }
          <Col xs={3}>
            <Button color="primary" onClick={() => onUserSearch(t, handleSearchResult)}>{t('general.actions.search')}</Button>
          </Col>
        </>
      }
    </Row>
  );

  function onSearchChange(propName, value) {
    searchChange({ ...userSearch, [propName]: value });
  };

  async function onUserSearch(lngStr, handleSearchResult) {
    try {
      var searchId = userSearch.id > 0 ? `userId=${userSearch.id}` : '';
      var searchLogin = userSearch.login ? `login=${userSearch.login}` : '';
      if (!searchId && !searchLogin) {
        handleSearchResult(null, lngStr('appSetup.admin.userSearchError'));
        return;
      }

      var cardData = null;
      var errorText = "";

      if (searchId) {
        try {
          cardData = await GetAsync(`/administration/getCard?${searchId}`);
        } catch (e) {
          cardData = null;
          errorText = e.message;
        }
      }

      if (cardData == null && searchLogin) {
        cardData = await GetAsync(`/administration/getCard?${searchLogin}`);
      }

      if (cardData == null && errorText) {
        handleSearchResult(null, errorText);
        return;
      }

      handleSearchResult(cardData, null);
    }
    catch (error) {
      handleSearchResult(null, error.message);
    }
  }
}