import React from 'react';
import { Popover, PopoverHeader, PopoverBody, Row, Col } from "reactstrap";
import '../../styling/Common.css';

export function UserCardModal({ userInfo, targetId, isOpen }) {

  return (
    <Popover placement="right" isOpen={isOpen} target={targetId}>
      <PopoverHeader >{userInfo?.legalName}</PopoverHeader>
      <PopoverBody>
        <p>
          {userInfo.surname}{' '}{userInfo.firstName}{' '}{userInfo.patronimic}
        </p>
        <div className="spaceTop">
          <p>Рост:{' '}{userInfo.height}</p>
          <p>Возраст:{' '}{userInfo.age}</p>
          <p>Вес:{' '}{userInfo.weight}</p>
        </div>
        {userInfo.CoachLegalName &&
          <Row>
            <Col xs={2}>Тренер:{' '}{userInfo.CoachLegalName}</Col>
          </Row>
        }
      </PopoverBody>
    </Popover>
  );
}
