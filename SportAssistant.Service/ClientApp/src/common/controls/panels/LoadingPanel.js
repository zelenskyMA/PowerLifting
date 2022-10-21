import { Spinner } from 'reactstrap';
import '../../../styling/Common.css';

export function LoadingPanel({ message = "Загрузка..." }) {
  return (
    <div className="spaceBorder">
      <Spinner size="sm" type="grow" />
      <span>
        <em>{'  '}{message}</em>
      </span>
    </div>
  );
}