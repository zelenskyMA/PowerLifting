import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

// Позволяет превращать классы в функциональные компоненты и юзать хуки в рамках них.
const withRouter = WrappedComponent => props => {
  const params = useParams();
  const navigate = useNavigate();
  const { t } = useTranslation(); // вытаскиваем вынкцию, которая в списке и называется 't'.

  return (
    <WrappedComponent
      {...props}
      params={params}
      navigate={navigate}
      lngStr={t}
    />
  );
};

export default withRouter;