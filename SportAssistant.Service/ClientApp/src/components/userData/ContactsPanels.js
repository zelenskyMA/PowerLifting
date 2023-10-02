import React from 'react';
import { useTranslation } from 'react-i18next';
import '../../styling/Common.css';

export function ContactsPanel({ contacts }) {
  const { t } = useTranslation();

  if (!contacts?.telNumber && !contacts?.telegram) {
    return (<p>{t('appSetup.user.noContacts')}</p>);
  }

  var telegramUrl = `https://t.me/${contacts.telegram}`;

  return (
    <>
      <p>{t('appSetup.user.contactMe')}:</p>

      <p>
        {contacts.telegram && <a href={telegramUrl} target="_blank" rel="noreferrer"><img src="/img/contacts/telegram_icon.png" alt="" width="30" height="30" /></a>}

        <span className="spaceLeft">{t('general.common.tel')}:{' '}{contacts.telNumber}</span>
      </p>
    </>
  );
}

export function MessengersPanel({ contacts }) {
  if (!contacts?.telegram) {
    return (<p> - </p>);
  }

  var telegramUrl = `https://t.me/${contacts.telegram}`;

  return (
    <p>
      {contacts.telegram &&
        <a href={telegramUrl} target="_blank" rel="noreferrer" onClick={(e) => e.stopPropagation()}><img src="/img/contacts/telegram_icon.png" alt="" width="30" height="30" /></a>}
    </p>
  );
}



