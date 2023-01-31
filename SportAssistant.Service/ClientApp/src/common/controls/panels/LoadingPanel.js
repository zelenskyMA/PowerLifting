import { Spinner } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import '../../../styling/Common.css';

export function LoadingPanel({ message = null }) {
  const { t } = useTranslation();
  if (!message) {
    message = t('appSetup.control.loading') + '...';
  }

  return (
    <div className="spaceBorder">
      <Spinner size="sm" type="grow" />
      <span>
        <em>{'  '}{message}</em>
      </span>
    </div>
  );
}