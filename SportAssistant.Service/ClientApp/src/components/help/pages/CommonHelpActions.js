import React from 'react';
import { useTranslation } from 'react-i18next';
import '../../../styling/Common.css';

export function GetAsHtml(helpStrId) {
  const { t } = useTranslation('help');
  return (<div dangerouslySetInnerHTML={{ __html: t(helpStrId, { interpolation: { escapeValue: false } }) }} />);
}

export function GetMsgWithUrl(url, text = "Ссылка на видео") {
  const { t } = useTranslation('help');
  return (<>{t('common.link')} <a href={url} target="_blank" rel="noreferrer">{text}</a></>);
}