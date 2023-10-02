import React from 'react';
import { Popover, PopoverHeader, PopoverBody, Row, Col } from "reactstrap";
import { useTranslation } from 'react-i18next';
import { ContactsPanel } from './ContactsPanels';
import '../../styling/Common.css';

export function UserCardModal({ userInfo, targetId, isOpen }) {
  const { t } = useTranslation();

  return (
    <Popover placement="right" isOpen={isOpen} target={targetId}>
      <PopoverHeader >{userInfo?.legalName}</PopoverHeader>
      <PopoverBody>
        <p>
          {userInfo.surname}{' '}{userInfo.firstName}{' '}{userInfo.patronimic}
        </p>
        <div className="spaceTop">
          <p>{t('appSetup.user.height')}:{' '}{userInfo.height}</p>
          <p>{t('appSetup.user.age')}:{' '}{userInfo.age}</p>
          <p>{t('appSetup.user.weight')}:{' '}{userInfo.weight}</p>
        </div>

        <div className="spaceTop">
          <ContactsPanel contacts={userInfo.contacts} />
        </div>

        {userInfo.CoachLegalName &&
          <Row>
            <Col xs={2}>{t('coaching.trainer') + ': ' + userInfo.CoachLegalName}</Col>
          </Row>
        }
      </PopoverBody>
    </Popover>
  );
}
